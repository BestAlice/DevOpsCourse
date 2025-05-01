using System.Security.Cryptography;
using BooksLab.Books;
using BooksLab.ConsoleCommands;
using DinoServer.Interfaces;
using DinoServer.Services;
using DinoServer.Users;
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
public class UserController : ControllerBase
{
    private readonly IGetBooksService _getBooksService;
    private readonly IAddBookService _addBookService;

    public UserController(
        IGetBooksService getBooksService,
        IAddBookService addBookService)
    {
        _getBooksService = getBooksService;
        _addBookService = addBookService;
    }

    [HttpGet("getusers")]
    public async Task<ActionResult<IEnumerable<User>>> Get()
    {
        var books = await _getBooksService.GetBooksAsync();
        return Ok(books);
    }

    [HttpPost("addscore")]
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
