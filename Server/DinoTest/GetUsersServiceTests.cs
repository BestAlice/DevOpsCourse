using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DinoServer.Services;

using DinoServer.Users;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DinoTest.Tests
{
  public class GetUsersServiceTests
  {
    private IDbContextFactory<UserContext> _factory = null!;
    private GetUsersService _service = null!;

    public GetUsersServiceTests()
    {
      var options = new DbContextOptionsBuilder<UserContext>()
          .UseInMemoryDatabase(databaseName: "TestDb_GetUsers")
          .Options;

      _factory = new DbContextFactory<UserContext>(options);

      // Добавляем тестовых пользователей
      using var context = _factory.CreateDbContext();
      context.Users.AddRange(new List<User>
            {
                new User { Id = 1, Name = "Alice" },
                new User { Id = 2, Name = "Bob" }
            });
      context.SaveChanges();

      _service = new GetUsersService(_factory);
    }

    [Fact]
    public async Task GetUsersAsync_ReturnsAllUsers()
    {
      var users = await _service.GetUsersAsync();
      Assert.Equal(2, users.Count());
    }
  }

  // Вспомогательная фабрика для тестов
  public class DbContextFactory<T> : IDbContextFactory<T> where T : DbContext
  {
    private readonly DbContextOptions<T> _options;

    public DbContextFactory(DbContextOptions<T> options)
    {
      _options = options;
    }

    public T CreateDbContext()
    {
      return (T)Activator.CreateInstance(typeof(T), _options)!;
    }
  }
}
