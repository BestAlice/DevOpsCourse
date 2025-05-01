using BooksLab.Books;
using BooksLab.Interface;
using DinoServer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DinoServer.Services;

public class SearchBooksByKeywordsService : ISearchBooksService
{
    private readonly IDbContextFactory<UserContext> _contextFactory;
    private readonly IBookSearch _bookSearch;

    public SearchBooksByKeywordsService(IDbContextFactory<UserContext> contextFactory, IBookSearch bookSearch)
    {
        _contextFactory = contextFactory;
        _bookSearch = bookSearch;
    }

    public async Task<IEnumerable<User>> SearchBooksAsync(int userId, string searchQuery)
    {
        await using var db = _contextFactory.CreateDbContext();
        db.UserId = userId;
        return await _bookSearch.SearchAsync(db, searchQuery, (User book) => "");
    }
}