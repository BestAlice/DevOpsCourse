using BooksLab.Books;
using DinoServer.Interfaces;
using DinoServer.Users;
using Microsoft.EntityFrameworkCore;

namespace DinoServer.Services;

public class AddBookService : IAddBookService
{
    private readonly IDbContextFactory<UserContext> _contextFactory;

    public AddBookService(IDbContextFactory<UserContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<User> AddBookAsync(User user, int userId)
    {
        if (user == null)
        {
            throw new ArgumentException("Book data is invalid.");
        }
        await using var db = _contextFactory.CreateDbContext();
        await db.AddBookAsync(user);
        return user;
    }
}