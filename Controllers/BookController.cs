using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementAPI.Dtos.Book;
using BookManagementAPI.Helpers;
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


        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] QueryObject queryObject)
        {
            var books = await _bookRepository.GetBooks(queryObject);
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