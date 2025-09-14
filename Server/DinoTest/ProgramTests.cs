using Xunit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using DinoServer.Users;
using Microsoft.EntityFrameworkCore;

namespace DinoTest.Tests
{
  public class ProgramTests
  {
    [Fact]
    public void Builder_CanCreateServices()
    {
      var builder = WebApplication.CreateBuilder();
      builder.Services.AddDbContextFactory<UserContext>(options =>
          options.UseInMemoryDatabase("TestDb"));

      builder.Services.AddScoped<DinoServer.Interfaces.IGetUsersService, DinoServer.Services.GetUsersService>();

      var app = builder.Build();
      var service = app.Services.GetService<DinoServer.Interfaces.IGetUsersService>();
      Assert.NotNull(service);
    }
  }
}
