using System.Security.Cryptography;
using BooksLab.Books;
using BooksLab.ConsoleCommands;
using DinoServer.Interfaces;
using DinoServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Query.ExpressionVisitors.Internal;
using SQLitePCL;

namespace DinoServer.Controllers;
/*
 *   Контроллер. Предназначен преимущественно для обработки запросов протокола HTTP:
 *  Get, Post, Put, Delete, Patch, Head, Options
 */
[ApiController]
[Route("/api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IGetBooksService _getBooksService;
    private readonly IEnumerable<ISearchBooksService> _searchBooksServices;
    private readonly IAddBookService _addBookService;

    public BookController(
        IGetBooksService getBooksService,
        IEnumerable<ISearchBooksService> searchBooksServices,
        IAddBookService addBookService)
    {
        _getBooksService = getBooksService;
        _searchBooksServices = searchBooksServices;
        _addBookService = addBookService;
    }

    [HttpGet("getbooks")]
    public async Task<ActionResult<IEnumerable<User>>> Get()
    {
        var books = await _getBooksService.GetBooksAsync();
        return Ok(books);
    }

    [HttpGet("searchby")]
    public async Task<ActionResult<IEnumerable<User>>> SearchBooks(int userId, string searchType, string searchQuery)
    {
        ISearchBooksService searchService = searchType switch
        {
            "title" => _searchBooksServices.OfType<SearchBooksByTitleService>().First(),
            "author" => _searchBooksServices.OfType<SearchBooksByAuthorService>().First(),
            "isbn" => _searchBooksServices.OfType<SearchBooksByIsbnService>().First(),
            "keywords" => _searchBooksServices.OfType<SearchBooksByKeywordsService>().First(),
            _ => throw new ArgumentException("Invalid search type.")
        };

        var books = await searchService.SearchBooksAsync(userId, searchQuery);
        return Ok(books);
    }

    [HttpPost("addbook")]
    public async Task<IActionResult> AddBook([FromBody] User user, [FromQuery] int userId)
    {
        try
        {
            var addedBook = await _addBookService.AddBookAsync(user, userId);
            return Ok(addedBook);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
