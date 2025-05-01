using BooksLab.Books;
using DinoServer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DinoServer.Services;

public class GetBooksService : IGetBooksService
{
    private readonly IDbContextFactory<UserContext> _contextFactory;

    public GetBooksService(IDbContextFactory<UserContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<IEnumerable<User>> GetBooksAsync()
    {
        await using var db = _contextFactory.CreateDbContext();
        return await db.Books.ToListAsync();
    }
}