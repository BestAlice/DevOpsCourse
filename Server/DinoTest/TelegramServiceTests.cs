using Xunit;
using DinoServer;
using DinoServer.Users;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DinoTest.Tests
{
  public class TelegramServiceTests
  {
    private readonly IDbContextFactory<UserContext> _contextFactory;

    public TelegramServiceTests()
    {
      var options = new DbContextOptionsBuilder<UserContext>()
          .UseInMemoryDatabase("TestDb_Telegram")
          .Options;

      _contextFactory = new TestDbContextFactory<UserContext>(options);
    }

    [Fact]
    public void Initialize_Should_SetContextFactory()
    {
      TelegramService.Initialize(_contextFactory);
      var task = TelegramService.SendMessage("Test");
      Assert.NotNull(task);
    }

    [Fact]
    public async Task SendMessage_Should_NotThrow_WhenNoSubscribers()
    {
      await TelegramService.SendMessage("Hello");
    }

    [Fact]
    public void Stop_Should_NotThrow()
    {
      TelegramService.Stop();
    }
  }
}
