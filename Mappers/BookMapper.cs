using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementAPI.Dtos.Book;
using BookManagementAPI.Models;

namespace BookManagementAPI.Mappers
{
    public static class BookMapper
    {
        public static BookDto ToBookDto(this Book BookModel)
        {
            return new BookDto
            {
                Id = BookModel.Id,
                Title = BookModel.Title,
                Author = BookModel.Author,
                ISBN = BookModel.ISBN,
                PublicationDate = BookModel.PublicationDate
            };
        }

        public static Book ToBookFromCreateBookRequestDto(this CreateBookRequestDto createBookRequestDto)
        {
            return new Book
            {
                Title = createBookRequestDto.Title,
                Author = createBookRequestDto.Author,
                ISBN = createBookRequestDto.ISBN,
                PublicationDate = createBookRequestDto.PublicationDate
            };
        }
    }
}