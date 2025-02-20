using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementAPI.Dtos.Book;
using BookManagementAPI.Interfaces;
using BookManagementAPI.Mappers;
using BookManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementAPI.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        private static List<Book> _books = new(){
            new Book{
                Id = 1,
                Title = "The Great Gatsby",
                Author = "F. Scott Fitzgerald",
                ISBN = "9780743273565",
                PublicationDate = new DateTime(1925, 4, 10)
            },
            new Book{
                Id = 2,
                Title = "To Kill a Mockingbird",
                Author = "Harper Lee",
                ISBN = "9780061120084",
                PublicationDate = new DateTime(1960, 7, 11)
            },
            new Book{
                Id = 3,
                Title = "1984",
                Author = "George Orwell",
                ISBN = "9780451524935",
                PublicationDate = new DateTime(1949, 6, 8)
            }
        };


        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _bookRepository.GetBooks();
            var bookDtos = books.Select(book => book.ToBookDto());
            return Ok(bookDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound();

            }
            var bookDto = book.ToBookDto();
            return Ok(bookDto);
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] CreateBookRequestDto createBookRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var book = createBookRequestDto.ToBookFromCreateBookRequestDto();
            book.Id = _books.Max(book => book.Id) + 1;
            _books.Add(book);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book.ToBookDto());
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] UpdateBookRequestDto updateBookRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var book = _books.FirstOrDefault(book => book.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            book.Title = updateBookRequestDto.Title;
            book.Author = updateBookRequestDto.Author;
            book.ISBN = updateBookRequestDto.ISBN;
            book.PublicationDate = updateBookRequestDto.PublicationDate;

            return Ok(book.ToBookDto());

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _books.FirstOrDefault(book => book.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            _books.Remove(book);
            return Ok("Book deleted");
        }


    }
}