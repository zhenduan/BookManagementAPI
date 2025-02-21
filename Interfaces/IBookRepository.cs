using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementAPI.Dtos.Book;
using BookManagementAPI.Helpers;
using BookManagementAPI.Models;

namespace BookManagementAPI.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetBooks(QueryObject queryObject);
        Task<Book?> GetBookById(int id);
        Task<Book> AddBook(Book book);
        Task<Book?> UpdateBook(int id, UpdateBookRequestDto book);
        Task<Book?> DeleteBook(int id);
    }
}