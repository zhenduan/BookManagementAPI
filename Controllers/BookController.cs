using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using BookManagementAPI.Dtos.Book;
using BookManagementAPI.Helpers;
using BookManagementAPI.Interfaces;
using BookManagementAPI.Mappers;
using BookManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BookManagementAPI.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMemoryCache _memoryCache;

        public BookController(IBookRepository bookRepository, IMemoryCache memoryCache)
        {
            _bookRepository = bookRepository;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] QueryObject queryObject)
        {

            // Generate a cache key based on the query parameters
            var cacheKey = CacheHelper.GenerateCacheKey(queryObject);

            // Try to get data from cache
            if (_memoryCache.TryGetValue(cacheKey, out List<BookDto> bookDtos))
            {
                return Ok(bookDtos);
            }
            else
            {
                // If not in cache, fetch from the repository
                var books = await _bookRepository.GetBooks(queryObject);
                bookDtos = books.Select(book => book.ToBookDto()).ToList();

                // Save data in cache with a dynamic key
                _memoryCache.Set(cacheKey, bookDtos, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

                return Ok(bookDtos);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            if (_memoryCache.TryGetValue(id, out BookDto bookDto))
            {
                return Ok(bookDto);
            }
            else
            {
                var book = await _bookRepository.GetBookById(id);
                if (book == null)
                {
                    return NotFound();

                }
                bookDto = book.ToBookDto();
                _memoryCache.Set(id, bookDto, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

                return Ok(bookDto);
            }

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