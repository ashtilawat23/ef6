import os
import requests
import logging
import json
import re
import time
import numpy as np
from github import Github
from langchain_openai import OpenAIEmbeddings
from langchain_text_splitters import CharacterTextSplitter
from pinecone import Pinecone, ServerlessSpec
from openai import OpenAI
from prompt import CODE_REVIEW_PROMPT

# Set up logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

# Set environment variables for API keys
api_key = os.getenv('OPEN_AI_KEY')
os.environ['PINECONE_API_KEY'] = os.getenv('PINECONE_API_KEY')

# Setup OpenAI client
openai_client = OpenAI(api_key=api_key)

# Initialize Pinecone client with API key
pc = Pinecone(api_key=os.getenv("PINECONE_API_KEY"))
index_name = "code-reviewer"

# Check if the index exists using list_indexes()
if index_name not in pc.list_indexes().names():
    logger.info(f"Index {index_name} does not exist. Creating new index.")
    pc.create_index(
        name=index_name,
        dimension=1536,  # OpenAI embedding dimension
        metric="cosine",
        spec=ServerlessSpec(cloud="aws", region=os.getenv("PINECONE_REGION"))
    )

# Wait for the index to be ready
while not pc.describe_index(index_name).status['ready']:
    time.sleep(1)

# Initialize embeddings
embeddings = OpenAIEmbeddings(openai_api_key=api_key)

# Get Pinecone index
index = pc.Index(index_name)

# Initialize GitHub client with access token
github_token = os.getenv('GITHUB_TOKEN')
github_client = Github(github_token)

# Initialize text splitter
text_splitter = CharacterTextSplitter(chunk_size=500, chunk_overlap=50)

# Function to embed and store text in Pinecone with metadata
def embed_and_store_text(text, metadata):
    docs = text_splitter.split_text(text)
    # Generate embeddings for each chunk
    embeddings_list = embeddings.embed_documents(docs)
    
    # Prepare vectors for upsert
    vectors = []
    for i, (doc, embedding) in enumerate(zip(docs, embeddings_list)):
        vector_id = f"{metadata['id']}-chunk-{i}"
        vectors.append({
            "id": vector_id,
            "values": embedding,
            "metadata": {**metadata, "text": doc}
        })
    
    # Upsert to Pinecone in batches
    batch_size = 100
    for i in range(0, len(vectors), batch_size):
        batch = vectors[i:i + batch_size]
        index.upsert(vectors=batch)

# Function to perform similarity search
def similarity_search(query, k=5):
    # Generate embedding for the query
    query_embedding = embeddings.embed_query(query)
    
    # Search in Pinecone
    results = index.query(
        vector=query_embedding,
        top_k=k,
        include_metadata=True
    )
    
    # Extract and return the text from the results
    return [match.metadata["text"] for match in results.matches]

# Fetch PR comments and add to Pinecone
def ingest_pr_comments(repo_name):
    repo = github_client.get_repo(repo_name)
    pulls = repo.get_pulls(state='closed')
    
    for pr in pulls:
        comments = pr.get_review_comments()
        for comment in comments:
            metadata = {
                "id": f"pr-{pr.number}-comment-{comment.id}",
                "type": "pr_comment",
                "pr_number": pr.number,
                "comment_author": comment.user.login
            }
            embed_and_store_text(comment.body, metadata)

# Fetch code base and add files to Pinecone
def ingest_code_base(repo_name):
    repo = github_client.get_repo(repo_name)
    contents = repo.get_contents("")
    process_directory(contents, repo)

def process_directory(contents, repo):
    for content in contents:
        if content.type == "file" and content.name.endswith((".py", ".js", ".java", ".cpp")):
            code = requests.get(content.download_url).text
            metadata = {"id": content.path, "file_name": content.name, "type": "code"}
            embed_and_store_text(code, metadata)
        elif content.type == "dir":
            process_directory(repo.get_contents(content.path), repo)

# Retrieve GitHub event data
def get_pr_details():
    event_path = os.getenv('GITHUB_EVENT_PATH')
    try:
        with open(event_path, 'r') as f:
            event_data = json.load(f)
        
        repo = os.getenv('GITHUB_REPOSITORY')
        pr_number = event_data['pull_request']['number']
        sha = event_data['pull_request']['head']['sha']
        
        logger.info(f"Repository: {repo}")
        logger.info(f"Pull Request Number: {pr_number}")
        logger.info(f"SHA: {sha}")
        
        return repo, pr_number, sha
    except Exception as e:
        logger.error(f"Error fetching PR details: {str(e)}")
        raise

# Fetch PR diff for specific files
def get_pr_diff():
    repo, pr_number, _ = get_pr_details()
    try:
        url = f"https://api.github.com/repos/{repo}/pulls/{pr_number}/files"
        headers = {
            'Authorization': f"Bearer {os.getenv('GITHUB_TOKEN')}",
            'Accept': 'application/vnd.github.v3+json',
        }
        
        response = requests.get(url, headers=headers)
        response.raise_for_status()
        files = response.json()
        
        # Include 'diff_hunk' in the structured data for each file
        processed_files = [
            {"filename": file["filename"], "patch": file.get("patch", ""), "diff_hunk": file.get("patch", "")}
            for file in files
        ]
        logger.info(f"Fetched PR diff for {len(files)} files.")
        return processed_files
    except Exception as e:
        logger.error(f"Error fetching PR diff: {str(e)}")
        raise


# Function to parse feedback into line-specific comments
def parse_feedback_into_comments(feedback, file_name):
    comments = []
    lines = feedback.split("\n")
    for line in lines:
        match = re.match(r"Line (\d+): (.+)", line)
        if match:
            line_number, comment_text = int(match.group(1)), match.group(2)
            comments.append({
                "file_name": file_name,
                "line": line_number,
                "comment": comment_text
            })
    logger.info(f"Parsed {len(comments)} comments from feedback for {file_name}.")
    return comments

def calculate_position_in_diff(file_name, line_number):
    # Fetch the diff data for the specific file
    files = get_pr_diff()
    file_patch = next((file["patch"] for file in files if file["filename"] == file_name), None)

    if not file_patch:
        logger.error(f"No patch found for file {file_name}")
        return None
    
    # Initialize variables to track position in the diff and current line number
    position = 1  # GitHub diff position is 1-based
    current_line_number = None

    # Process each line in the patch
    for line in file_patch.splitlines():
        # Identify line number headers, e.g., "@@ -15,7 +15,8 @@"
        header_match = re.match(r'^@@ -\d+(?:,\d+)? \+(\d+)(?:,\d+)? @@', line)
        if header_match:
            # Update `current_line_number` to the start of added lines in this chunk
            current_line_number = int(header_match.group(1))
            continue

        # For lines added in the diff (those prefixed with '+')
        if line.startswith('+') and not line.startswith('+++'):
            # Check if this line matches the desired line number
            if current_line_number == line_number:
                logger.info(f"Matched line {line_number} in file {file_name} at diff position {position}")
                return position
            current_line_number += 1  # Increment line number for added lines
            position += 1  # Increment position in the diff

        # For removed lines (those prefixed with '-'), just increment line number
        elif line.startswith('-') and not line.startswith('---'):
            current_line_number += 1

    # If no position found, log a warning and return None
    logger.warning(f"No matching position found in diff for line {line_number} in {file_name}")
    return 1

def review_code_with_rag(files):
    grouped_comments = {}  # Dictionary to hold comments per file

    for file in files:
        file_name = file['filename']
        patch = file.get('patch', '')

        # Retrieve relevant context from Pinecone based on the patch
        similar_docs = similarity_search(patch, k=5)
        relevant_context = "\n".join(similar_docs)
        
        prompt = CODE_REVIEW_PROMPT.format(
            file_name=file_name,
            patch=patch,
            relevant_context=relevant_context
        )

        try:
            completion = openai_client.chat.completions.create(
                model="gpt-4o",
                messages=[
                    {"role": "system", "content": "You are a senior software engineer with 10+ years of experience in code review and software architecture. You excel at identifying code quality issues, security vulnerabilities, and performance bottlenecks. Your feedback is always actionable, backed by concrete examples and industry best practices. You communicate technical concepts clearly and concisely, focusing on high-impact issues that matter most for code maintainability and reliability. You have deep expertise in secure coding practices, performance optimization, and scalable system design."},
                    {"role": "user", "content": prompt},
                ],
            )
            
            feedback = completion.choices[0].message.content
            logger.info(f"Received feedback for {file_name}: {feedback[:100]}...")
            grouped_comments[file_name] = feedback

        except Exception as e:
            logger.error(f"Error during OpenAI API call: {str(e)}")
            raise

    logger.info(f"Generated grouped comments for {len(grouped_comments)} files.")
    return grouped_comments

def post_line_comment(repo, pr_number, file_name, position, comment):
    # Skip files in tests directory or with "test" in the name
    if "tests/" in file_name.lower() or "test" in file_name.lower():
        logger.info(f"Skipping comment on test file: {file_name}")
        return None
        
    url = f"https://api.github.com/repos/{repo}/pulls/{pr_number}/comments"
    headers = {
        'Authorization': f"Bearer {os.getenv('PERSONAL_GITHUB_TOKEN')}",
        'Content-Type': 'application/json',
        'Accept': 'application/vnd.github.v3+json'
    }
    payload = {
        "body": comment,
        "commit_id": get_pr_details()[2],
        "path": file_name,
        "side": "RIGHT",
        "position": position  # This is relative to the diff, not the line number
    }

    logger.info(f"Attempting to post comment on {file_name} at position {position}: {comment}")

    try:
        response = requests.post(url, headers=headers, json=payload)
        response.raise_for_status()
        logger.info(f"Comment posted successfully on {file_name} at position {position}")
        return response.json()
    except requests.exceptions.RequestException as e:
        logger.error(f"Error posting comment on {file_name} at position {position}: {str(e)}")
        logger.error(f"Response status: {response.status_code}")
        logger.error(f"Response text: {response.text}")
        raise



# Main execution
if __name__ == "__main__":
    try:
        repo, pr_number, sha = get_pr_details()
        
        # Ingest existing code base and PR comments for RAG (only run once)
        ingest_code_base(repo)
        ingest_pr_comments(repo)
        
        # Fetch structured PR diff data
        files = get_pr_diff()
        
        # Perform code review with RAG, receiving grouped comments by file
        grouped_comments = review_code_with_rag(files)
        
        # Post grouped comments on the PR, per file
        if grouped_comments:
            for file_name, comment_text in grouped_comments.items():
                # Post the entire grouped comment as a single comment at the top of the file (or a default location)
                post_line_comment(repo, pr_number, file_name, 1, comment_text)
        else:
            logger.info("No comments to post.")
        
        logger.info("AI Code Review with grouped comments per file completed successfully.")
    except Exception as e:
        logger.error(f"Error during AI Code Review: {str(e)}")
        raise