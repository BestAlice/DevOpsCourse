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
        
        builder.Services.AddScoped<IGetBooksService, GetBooksService>();
        builder.Services.AddScoped<IAddBookService, AddBookService>();

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
    
    private static X509Certificate2 LoadCertificate()
    {
        var certPath = Path.Combine(AppContext.BaseDirectory, "Certificates", "cert.pem");
        var keyPath = Path.Combine(AppContext.BaseDirectory, "Certificates", "private_key.pem");

        if (!File.Exists(certPath))
        {
            throw new FileNotFoundException($"Certificate file not found: {certPath}");
        }

        if (!File.Exists(keyPath))
        {
            throw new FileNotFoundException($"Key file not found: {keyPath}");
        }

        using var certStream = new FileStream(certPath, FileMode.Open, FileAccess.Read);
        using var keyStream = new FileStream(keyPath, FileMode.Open, FileAccess.Read);

        var cert = new X509Certificate2(certStream.ReadFully());
        var rsa = LoadPrivateKey(keyStream);

        return cert.CopyWithPrivateKey(rsa);
    }

    private static RSA LoadPrivateKey(Stream keyStream)
    {
        using var reader = new StreamReader(keyStream);
        var pem = reader.ReadToEnd();

        // Remove PEM headers/footers and whitespace
        pem = Regex.Replace(pem, @"-----BEGIN PRIVATE KEY-----", "");
        pem = Regex.Replace(pem, @"-----END PRIVATE KEY-----", "");
        pem = Regex.Replace(pem, @"\s+", ""); // Remove all whitespace, including newlines

        try
        {
            var rsa = RSA.Create();
            // Try importing as PKCS#8 first (since your key is in PKCS#8 format)
            rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(pem), out _);
            return rsa;
        }
        catch (CryptographicException ex)
        {
            // Fallback to PKCS#1 if PKCS#8 fails (though your key is PKCS#8)
            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(pem), out _);
            return rsa;
        }
    }
}

public static class StreamExtensions
{
    public static byte[] ReadFully(this Stream input)
    {
        using var ms = new MemoryStream();
        input.CopyTo(ms);
        return ms.ToArray();
    }
}
    