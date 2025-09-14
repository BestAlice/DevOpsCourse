using Xunit;
using DinoServer.Users;

namespace DinoTest.Tests
{
  public class UserTests
  {
    [Fact]
    public void UserName_ShouldNotBeNull()
    {
      var user = new User { Name = "Alice" };
      Assert.NotNull(user.Name);
    }

    [Fact]
    public void UserName_ShouldBeAlice()
    {
      var user = new User { Name = "Alice" };
      Assert.Equal("Alice", user.Name);
    }
  }
}
