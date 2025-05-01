using BooksLab.Books;

namespace DinoServer.Interfaces;

public interface IGetBooksService
{
    Task<IEnumerable<User>> GetBooksAsync();
}