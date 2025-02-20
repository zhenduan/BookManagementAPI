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
        public async Task<IActionResult> CreateBook([FromBody] CreateBookRequestDto createBookRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var book = createBookRequestDto.ToBookFromCreateBookRequestDto();
            await _bookRepository.AddBook(book);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book.ToBookDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookRequestDto updateBookRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var bookUpdate = await _bookRepository.UpdateBook(id, updateBookRequestDto);
            if (bookUpdate == null)
            {
                return NotFound();
            }
            return Ok(bookUpdate.ToBookDto());

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var bookDelete = await _bookRepository.DeleteBook(id);
            if (bookDelete == null)
            {
                return NotFound();
            }
            return Ok("Book deleted");
        }


    }
}