using Microsoft.EntityFrameworkCore;
using System;

namespace DinoTest.Tests
{
  // Фабрика, которая создаёт новый контекст при каждом вызове
  public class TestDbContextFactory<TContext> : IDbContextFactory<TContext> where TContext : DbContext
  {
    private readonly DbContextOptions<TContext> _options;

    public TestDbContextFactory(DbContextOptions<TContext> options)
    {
      _options = options;
    }

    public TContext CreateDbContext()
    {
      // Создаём новый экземпляр контекста с переданными опциями
      return (TContext)Activator.CreateInstance(typeof(TContext), _options)!;
    }
  }
}
