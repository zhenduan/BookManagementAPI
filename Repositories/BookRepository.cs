using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementAPI.Data;
using BookManagementAPI.Dtos.Book;
using BookManagementAPI.Helpers;
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

        public async Task<Book?> DeleteBook(int id)
        {
            var bookToDelete = await _context.Books.FindAsync(id);
            if (bookToDelete == null)
            {
                return null;
            }

            _context.Books.Remove(bookToDelete);
            await _context.SaveChangesAsync();
            return bookToDelete;
        }

        public async Task<Book?> GetBookById(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<List<Book>> GetBooks(QueryObject queryObject)
        {
            var books = _context.Books.AsQueryable();
            if (!string.IsNullOrWhiteSpace(queryObject.SearchQuery))
            {
                books = books.Where(b => b.Title.Contains(queryObject.SearchQuery) ||
                                         b.Author.Contains(queryObject.SearchQuery) ||
                                         b.ISBN.Contains(queryObject.SearchQuery));
            }

            if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
            {
                bool isDescending = queryObject.SortOrder?.ToLower() == "desc";
                books = queryObject.SortBy switch
                {
                    "title" => isDescending ? books.OrderByDescending(b => b.Title) : books.OrderBy(b => b.Title),
                    "author" => isDescending ? books.OrderByDescending(b => b.Author) : books.OrderBy(b => b.Author),
                    _ => books
                };
            }
            books = books.Skip((queryObject.PageNumber - 1) * queryObject.PageSize)
                         .Take(queryObject.PageSize);
            return await books.ToListAsync();
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