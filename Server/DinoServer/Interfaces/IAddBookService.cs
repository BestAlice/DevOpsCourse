using BooksLab.Books;

namespace DinoServer.Interfaces;

public interface IAddBookService
{
    Task<User> AddBookAsync(User user, int userId);
}