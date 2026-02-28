using Microsoft.AspNetCore.Mvc;
using uppgift2.service.DTOs;
using uppgift2.service.Models;
using uppgift2.service.Services;

namespace uppgift2.api.Controllers;

/// <summary>
/// CRUD-operationer för böcker
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>
    /// Hämtar alla böcker
    /// </summary>
    /// <returns>Lista över alla böcker i biblioteket</returns>
    /// <response code="200">Returnerar listan över alla böcker</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    /// <summary>
    /// Hämtar en specifik bok
    /// </summary>
    /// <param name="id">Bokens ID</param>
    /// <returns>Boken med angivet ID</returns>
    /// <response code="200">Returnerar boken</response>
    /// <response code="404">Boken hittades inte</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBook(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        
        if (book == null)
            return NotFound($"Bok med ID {id} hittades inte.");
        
        return Ok(book);
    }

    /// <summary>
    /// Skapar en ny bok
    /// </summary>
    /// <param name="createBookDto">Bokens information</param>
    /// <returns>Den nyskapade boken</returns>
    /// <response code="201">Boken har skapats</response>
    /// <response code="400">Ogiltig data</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookDto createBookDto)
    {
        var book = new Book
        {
            Title = createBookDto.Title,
            Author = createBookDto.Author,
            ISBN = createBookDto.ISBN,
            Pages = createBookDto.Pages,
            PublishedYear = createBookDto.PublishedYear
        };

        var createdBook = await _bookService.CreateBookAsync(book);
        return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
    }

    /// <summary>
    /// Uppdaterar en befintlig bok
    /// </summary>
    /// <param name="id">Bokens ID</param>
    /// <param name="updateBookDto">Uppdaterad information</param>
    /// <returns>Den uppdaterade boken</returns>
    /// <response code="200">Boken har uppdaterats</response>
    /// <response code="404">Boken hittades inte</response>
    /// <response code="400">Ogiltig data</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookDto updateBookDto)
    {
        var book = new Book
        {
            Title = updateBookDto.Title,
            Author = updateBookDto.Author,
            ISBN = updateBookDto.ISBN,
            Pages = updateBookDto.Pages,
            PublishedYear = updateBookDto.PublishedYear
        };

        var updatedBook = await _bookService.UpdateBookAsync(id, book);
        
        if (updatedBook == null)
            return NotFound($"Bok med ID {id} hittades inte.");
        
        return Ok(updatedBook);
    }

    /// <summary>
    /// Tar bort en bok
    /// </summary>
    /// <param name="id">Bokens ID</param>
    /// <returns>Inget innehåll</returns>
    /// <response code="204">Boken har tagits bort</response>
    /// <response code="404">Boken hittades inte</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var result = await _bookService.DeleteBookAsync(id);
        
        if (!result)
            return NotFound($"Bok med ID {id} hittades inte.");
        
        return NoContent();
    }
}
