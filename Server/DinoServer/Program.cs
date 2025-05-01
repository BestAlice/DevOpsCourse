using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using BooksLab;
using BooksLab.Books;
using BooksLab.ConsoleCommands;
using BooksLab.Interface;
using DinoServer.Interfaces;
using DinoServer.Services;
using DinoServer.Users;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace DinoServer;
class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        string con = "server=localhost;user=root;password=password;database=DinoDB;";
        var version = new MySqlServerVersion(new Version(8, 0, 11));
        builder.Services.AddDbContextFactory<UserContext>(options => options.UseMySql(con, version));
        
        builder.Services.AddScoped<IGetUsersService, GetUsersService>();
        builder.Services.AddScoped<IAddUserService, AddUserService>();

        builder.Services.AddControllers();
        
        // Настройка Kestrel для использования самоподписанного сертификата
        /*builder.WebHost.UseKestrel(options =>
        {
            options.ListenAnyIP(7151, listenOptions =>
            {
                listenOptions.UseHttps(LoadCertificate());
            });
        });*/

        var app = builder.Build();
        app.UseDeveloperExceptionPage();
        app.UseStaticFiles();
        app.UseDefaultFiles();
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapControllers();
        
        app.Run();
    }
}

    