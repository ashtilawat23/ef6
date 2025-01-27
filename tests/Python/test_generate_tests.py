import unittest
from unittest.mock import patch, MagicMock, mock_open
import os
import sys
import json
from datetime import datetime
from your_module import (get_pr_details, get_pr_diff, get_code_files, 
                         read_file_content, get_language_from_extension, 
                         generate_unit_tests, write_unit_test_file, 
                         get_test_file_name, process_repository)

class TestYourModule(unittest.TestCase):

    @patch.dict(os.environ, {'GITHUB_EVENT_PATH': 'mock_event_path.json', 'GITHUB_REPOSITORY': 'test/repo'})
    @patch("builtins.open", new_callable=mock_open, read_data='{"pull_request": {"number": 1, "head": {"sha": "abc123"}}}')
    def test_get_pr_details_success(self, mock_open):
        repo, pr_number, sha = get_pr_details()
        self.assertEqual(repo, 'test/repo')
        self.assertEqual(pr_number, 1)
        self.assertEqual(sha, 'abc123')

    @patch.dict(os.environ, {'GITHUB_EVENT_PATH': 'mock_event_path.json'})
    @patch("builtins.open", new_callable=mock_open)
    def test_get_pr_details_file_error(self, mock_open):
        mock_open.side_effect = FileNotFoundError
        with self.assertRaises(Exception):
            get_pr_details()

    @patch('your_module.github_client')
    def test_get_pr_diff_success(self, mock_github_client):
        mock_repo = MagicMock()
        mock_pr = MagicMock()
        mock_file = MagicMock()
        mock_file.filename = 'test.py'
        mock_file.status = 'modified'
        mock_file.additions = 10
        mock_file.deletions = 5
        mock_pr.get_files.return_value = [mock_file]
        mock_repo.get_pull.return_value = mock_pr
        mock_github_client.get_repo.return_value = mock_repo

        with patch('your_module.get_pr_details', return_value=('test/repo', 1, 'abc123')):
            result = get_pr_diff()
            self.assertEqual(len(result), 1)
            self.assertEqual(result[0]['filename'], 'test.py')

    @patch('your_module.github_client')
    def test_get_pr_diff_error(self, mock_github_client):
        mock_github_client.get_repo.side_effect = Exception("Error")
        with patch('your_module.get_pr_details', return_value=('test/repo', 1, 'abc123')):
            with self.assertRaises(Exception):
                get_pr_diff()

    @patch('os.path.exists', return_value=True)
    @patch('your_module.get_pr_diff', return_value=[{'filename': 'test.py'}])
    def test_get_code_files_success(self, mock_get_pr_diff, mock_exists):
        result = get_code_files('.')
        self.assertEqual(len(result), 1)
        self.assertTrue(result[0].endswith('test.py'))

    @patch('os.path.exists', return_value=False)
    @patch('your_module.get_pr_diff', return_value=[{'filename': 'test.py'}])
    def test_get_code_files_file_not_found(self, mock_get_pr_diff, mock_exists):
        result = get_code_files('.')
        self.assertEqual(len(result), 0)

    @patch('builtins.open', new_callable=mock_open, read_data='print("Hello, World!")')
    def test_read_file_content(self, mock_open):
        content = read_file_content('test.py')
        self.assertEqual(content, 'print("Hello, World!")')

    def test_get_language_from_extension(self):
        self.assertEqual(get_language_from_extension('.py'), 'Python')
        self.assertIsNone(get_language_from_extension('.unknown'))

    @patch('your_module.openai.chat.completions.create')
    def test_generate_unit_tests_success(self, mock_create):
        mock_create.return_value.choices[0].message.content = 'def test_function(): pass'
        code = generate_unit_tests('def function(): pass', 'Python')
        self.assertIn('def test_function', code)

    @patch('your_module.openai.chat.completions.create', side_effect=Exception("API Error"))
    def test_generate_unit_tests_api_error(self, mock_create):
        with self.assertRaises(Exception):
            generate_unit_tests('def function(): pass', 'Python')

    @patch('builtins.open', new_callable=mock_open)
    def test_write_unit_test_file(self, mock_open):
        write_unit_test_file('test_file.py', 'print("Hello, World!")')
        mock_open.assert_called_once_with('test_file.py', 'w', encoding='utf-8')

    def test_get_test_file_name(self):
        self.assertEqual(get_test_file_name('example.py', 'Python'), 'test_example.py')

    @patch('your_module.get_code_files', return_value=['test.py'])
    @patch('your_module.read_file_content', return_value='print("Hello, World!")')
    @patch('your_module.generate_unit_tests', return_value='def test_example(): pass')
    @patch('your_module.write_unit_test_file')
    def test_process_repository(self, mock_write, mock_generate, mock_read, mock_get_code_files):
        with patch('os.makedirs'), patch('os.path.exists', return_value=False):
            process_repository('.')
            mock_write.assert_called_once()

if __name__ == '__main__':
    unittest.main()