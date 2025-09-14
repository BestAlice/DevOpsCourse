using System.Threading.Tasks;
using DinoServer.Services;
using DinoServer.Users;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DinoTest.Tests
{
  public class AddUserServiceTests
  {
    private IDbContextFactory<UserContext> _factory = null!;
    private AddUserService _service = null!;

    public AddUserServiceTests()
    {
      var options = new DbContextOptionsBuilder<UserContext>()
          .UseInMemoryDatabase("TestDb_AddUser")
          .Options;

      _factory = new TestDbContextFactory<UserContext>(options);
      _service = new AddUserService(_factory);
    }

    [Fact]
    public async Task AddUser_ShouldAddUser()
    {
      var user = new User { Id = 10, Name = "Charlie", Score = 0 };
      await _service.AddUserAsync(user, 1);

      await using var context = _factory.CreateDbContext();
      var addedUser = await context.Users.FindAsync(10);
      Assert.NotNull(addedUser);
      Assert.Equal("Charlie", addedUser!.Name);
    }
  }
}
