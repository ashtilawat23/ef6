using System;
using System.Threading.Tasks;
using EF6Demo.Models;
using EF6Demo.Repositories;

namespace EF6Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                using (var repository = new BookRepository())
                {
                    // Add sample books
                    Console.WriteLine("Adding sample books...");
                    await AddSampleBooks(repository);

                    // Get all books
                    Console.WriteLine("\nAll available books:");
                    var books = await repository.GetAllBooksAsync();
                    foreach (var book in books)
                    {
                        Console.WriteLine($"{book.Title} by {book.Author} - Rating: {book.Rating:F1}, Price: ${book.Price:F2}");
                    }

                    // Get highly rated books (4.0 and above)
                    Console.WriteLine("\nHighly rated books (4.0+ stars):");
                    var highlyRated = await repository.GetBooksByRatingRangeAsync(4.0m, 5.0m);
                    foreach (var book in highlyRated)
                    {
                        Console.WriteLine($"{book.Title} by {book.Author} - Rating: {book.Rating:F1}");
                    }

                    // Update a book's stock quantity
                    var firstBook = await repository.GetBookByIdAsync(1);
                    if (firstBook != null)
                    {
                        Console.WriteLine($"\nUpdating stock quantity for {firstBook.Title}...");
                        await repository.UpdateStockQuantityAsync(firstBook.BookId, 50);
                    }

                    // Look up a book by ISBN
                    var isbnToFind = "978-0123456789";
                    Console.WriteLine($"\nLooking up book by ISBN: {isbnToFind}");
                    var bookByISBN = await repository.GetBookByISBNAsync(isbnToFind);
                    if (bookByISBN != null)
                    {
                        Console.WriteLine($"Found: {bookByISBN.Title} by {bookByISBN.Author}");
                    }

                    // Soft delete a book
                    Console.WriteLine("\nSoft deleting a book...");
                    await repository.DeleteBookAsync(2);

                    // Show remaining available books
                    Console.WriteLine("\nRemaining available books:");
                    books = await repository.GetAllBooksAsync();
                    foreach (var book in books)
                    {
                        Console.WriteLine($"{book.Title} by {book.Author} - Stock: {book.StockQuantity}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static async Task AddSampleBooks(BookRepository repository)
        {
            var books = new[]
            {
                new Book
                {
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    ISBN = "978-0123456789",
                    Price = 14.99m,
                    PublicationDate = new DateTime(1925, 4, 10),
                    Description = "A story of the fabulously wealthy Jay Gatsby and his love for the beautiful Daisy Buchanan.",
                    Rating = 4.5m,
                    StockQuantity = 25
                },
                new Book
                {
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    ISBN = "978-0987654321",
                    Price = 12.99m,
                    PublicationDate = new DateTime(1960, 7, 11),
                    Description = "The story of young Scout Finch and her father Atticus in a racially divided Southern town.",
                    Rating = 4.8m,
                    StockQuantity = 30
                },
                new Book
                {
                    Title = "1984",
                    Author = "George Orwell",
                    ISBN = "978-1234567890",
                    Price = 11.99m,
                    PublicationDate = new DateTime(1949, 6, 8),
                    Description = "A dystopian novel about totalitarianism and surveillance.",
                    Rating = 4.6m,
                    StockQuantity = 20
                }
            };

            foreach (var book in books)
            {
                await repository.AddBookAsync(book);
            }
        }
    }
} 