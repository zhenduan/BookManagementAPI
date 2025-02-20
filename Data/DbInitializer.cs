using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementAPI.Models;

namespace BookManagementAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Books.Any())
            {
                return;
            }

            var books = new Book[]
            {
                new Book{Title="The Great Gatsby", Author="F. Scott Fitzgerald", ISBN="9780743273565", PublicationDate=DateTime.Parse("1925-04-10")},
                new Book{Title="To Kill a Mockingbird", Author="Harper Lee", ISBN="9780061120084", PublicationDate=DateTime.Parse("1960-07-11")},
                new Book{Title="1984", Author="George Orwell", ISBN="9780451524935", PublicationDate=DateTime.Parse("1949-06-08")},
                new Book{Title="Pride and Prejudice", Author="Jane Austen", ISBN="9780141439518", PublicationDate=DateTime.Parse("1813-01-28")},
                new Book{Title="The Catcher in the Rye", Author="J.D. Salinger", ISBN="9780316769488", PublicationDate=DateTime.Parse("1951-07-16")},
                new Book{Title="The Hobbit", Author="J.R.R. Tolkien", ISBN="9780618260300", PublicationDate=DateTime.Parse("1937-09-21")},
                new Book{Title="The Lord of the Rings", Author="J.R.R. Tolkien", ISBN="9780618346257", PublicationDate=DateTime.Parse("1954-07-29")},
                new Book{Title="Animal Farm", Author="George Orwell", ISBN="9780451526342", PublicationDate=DateTime.Parse("1945-08-17")},
                new Book{Title="Brave New World", Author="Aldous Huxley", ISBN="9780060850524", PublicationDate=DateTime.Parse("1932-10-27")},
                new Book{Title="The Grapes of Wrath", Author="John Steinbeck", ISBN="9780143039433", PublicationDate=DateTime.Parse("1939-04-14")}
            };

            foreach (Book b in books)
            {
                context.Books.Add(b);
            }

            context.SaveChanges();
        }
    }
}