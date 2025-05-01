using BooksLab.Books;
using DinoServer.Users;

namespace DinoServer.Interfaces;

public interface IGetBooksService
{
    Task<IEnumerable<User>> GetBooksAsync();
}