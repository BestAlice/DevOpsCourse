namespace BooksLab.Interface;

using BooksLab.Books;

public interface IBookSearch
{
     public Task<List<Book>> SearchAsync(BookContext context, string query, Func<Book, string> field);
     
}
/*
public interface IBookSearch
{
     Task<List<Book>> SearchAsync(int userId, string query, SearchType searchType);
     Task<List<Book>> SearchByTitle(int userId, string title);
     Task<List<Book>> SearchByISBN(int userId, string isbn);
     Task<List<Book>> SearchByAuthor(int userId, string author);
}
*/