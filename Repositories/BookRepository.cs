using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementAPI.Data;
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
        public Task<Book> AddBook(Book book)
        {
            throw new NotImplementedException();
        }

        public Task<Book?> DeleteBook(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Book?> GetBookById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Book>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        public Task<Book?> UpdateBook(int id, Book book)
        {
            throw new NotImplementedException();
        }
    }
}