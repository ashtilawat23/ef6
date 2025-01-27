using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using EF6Demo.Data;
using EF6Demo.Models;

namespace EF6Demo.Repositories
{
    public class BookRepository : IDisposable
    {
        private readonly BookContext _context;
        private bool _disposed = false;

        public BookRepository()
        {
            _context = new BookContext();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<Book> GetBookByISBNAsync(string isbn)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books
                .Where(b => b.IsAvailable)
                .OrderBy(b => b.Title)
                .ToListAsync();
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            book.IsAvailable = false;  // Soft delete
            return await UpdateBookAsync(book);
        }

        public async Task<IEnumerable<Book>> GetBooksByRatingRangeAsync(decimal minRating, decimal maxRating)
        {
            return await _context.Books
                .Where(b => b.IsAvailable && b.Rating >= minRating && b.Rating <= maxRating)
                .OrderByDescending(b => b.Rating)
                .ToListAsync();
        }

        public async Task<bool> UpdateStockQuantityAsync(int bookId, int quantity)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null) return false;

            book.StockQuantity = quantity;
            book.IsAvailable = quantity > 0;
            return await UpdateBookAsync(book);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
} 