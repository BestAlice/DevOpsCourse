using BooksLab.Books;
using BooksLab.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksLab.ConsoleCommands
{
    public class BookSearch : IBookSearch
    {
        private IBookSearch _bookSearchImplementation;

        public async Task<List<Book>> SearchAsync(BookContext context, string query, Func<Book, string> field)
        {
            return context.Books.AsEnumerable().Where(book => field(book).ToLower().Contains(query.ToLower())).ToList();
        }

        public async Task<List<Book>> SearchByTitleAsync(BookContext context, string query)
        {
            return await SearchAsync(context, query, book => book.Title);
        }

        public async Task<List<Book>> SearchByISBNAsync(BookContext context, string query)
        {
            return await SearchAsync(context, query, book => book.ISBN);
        }

        public async Task<List<Book>> SearchByAuthorAsync(BookContext context, string query)
        {
            return await SearchAsync(context, query, book => book.Author);
        }

        public async Task<List<Book>> SearchBooksAsync(BookContext context, string query, string searchType)
        { 
            return searchType.ToLower() switch
            {
                "title" => await SearchByTitleAsync(context, query),
                "isbn" => await SearchByISBNAsync(context, query),
                "author" => await SearchByAuthorAsync(context, query),
                _ => throw new ArgumentException("Invalid search type")
            };
        }
    }
}