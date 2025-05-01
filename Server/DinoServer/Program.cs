using BooksLab;
using BooksLab.Books;
using BooksLab.ConsoleCommands;
using BooksLab.Interface;
using DinoServer.Interfaces;
using DinoServer.Services;
using DinoServer.Users;
using Microsoft.EntityFrameworkCore;

namespace DinoServer;
class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        string con = "server=localhost;user=root;password=password;database=DinoDB;";
        var version = new MySqlServerVersion(new Version(8, 0, 11));
        builder.Services.AddDbContextFactory<UserContext>(options => options.UseMySql(con, version));
        
        builder.Services.AddScoped<IGetBooksService, GetBooksService>();
        builder.Services.AddScoped<IAddBookService, AddBookService>();

        builder.Services.AddControllers();
        

        var app = builder.Build();
        app.UseDeveloperExceptionPage();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseDefaultFiles();
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
        app.Run();
    }
}
    