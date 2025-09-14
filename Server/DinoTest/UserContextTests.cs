using Xunit;
using DinoServer.Users;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;


public class UserContextTests
{
  private readonly UserContext _context;

  public UserContextTests()
  {
    var options = new DbContextOptionsBuilder<UserContext>()
                      .UseInMemoryDatabase("TestDB_Context")
                      .Options;
    _context = new UserContext(options);

    // Заполним тестовые данные
    _context.Users.AddRange(
        new User { Name = "Alice", Score = 5 },
        new User { Name = "Bob", Score = 10 }
    );
    _context.SaveChanges();
  }

  [Fact]
  public void Context_ShouldContainUsers()
  {
    var users = _context.Users.ToList();
    Assert.Equal(2, users.Count);
    Assert.Contains(users, u => u.Name == "Alice");
    Assert.Contains(users, u => u.Name == "Bob");
  }
}
