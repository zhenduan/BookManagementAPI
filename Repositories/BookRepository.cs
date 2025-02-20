using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementAPI.Data;
using BookManagementAPI.Dtos.Book;
using BookManagementAPI.Interfaces;
using BookManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagementAPI.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Book> AddBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public Task<Book?> DeleteBook(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Book?> GetBookById(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<List<Book>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> UpdateBook(int id, UpdateBookRequestDto book)
        {
            var bookToUpdate = await _context.Books.FindAsync(id);
            if (bookToUpdate == null)
            {
                return null;
            }

            bookToUpdate.Title = book.Title;
            bookToUpdate.Author = book.Author;
            bookToUpdate.ISBN = book.ISBN;
            bookToUpdate.PublicationDate = book.PublicationDate;

            await _context.SaveChangesAsync();
            return bookToUpdate;
        }
    }
}