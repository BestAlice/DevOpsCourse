using BooksLab.Books;

namespace DinoServer.Interfaces;

public interface ISearchBooksService
{
    Task<IEnumerable<User>> SearchBooksAsync(int userId, string searchQuery);
}