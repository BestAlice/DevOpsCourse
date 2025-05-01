using BooksLab.Books;
using DinoServer.Users;

namespace DinoServer.Interfaces;

public interface IAddBookService
{
    Task<User> AddBookAsync(User user, int userId);
}