using Microsoft.AspNetCore.Mvc;
using LibraryService.Grpc.Books;

namespace uppgift2.api.Controllers;

/// <summary>
/// CRUD-operationer för böcker
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BookService.BookServiceClient _grpcClient;

    public BooksController(BookService.BookServiceClient grpcClient)
    {
        _grpcClient = grpcClient;
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
        var request = new GetAllBooksRequest();
        var response = await _grpcClient.GetAllBooksAsync(request);
        
        return Ok(response.Books);
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
        var request = new GetBookRequest { Id = id };
        var response = await _grpcClient.GetBookAsync(request);
        
        if (!response.Found)
            return NotFound($"Bok med ID {id} hittades inte.");
        
        return Ok(response.Book);
    }

    /// <summary>
    /// Skapar en ny bok
    /// </summary>
    /// <param name="createBookRequest">Bokens information</param>
    /// <returns>Den nyskapade boken</returns>
    /// <response code="201">Boken har skapats</response>
    /// <response code="400">Ogiltig data</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookRequest createBookRequest)
    {
        var response = await _grpcClient.CreateBookAsync(createBookRequest);
        
        return CreatedAtAction(nameof(GetBook), new { id = response.Book.Id }, response.Book);
    }

    /// <summary>
    /// Uppdaterar en befintlig bok
    /// </summary>
    /// <param name="id">Bokens ID</param>
    /// <param name="updateBookRequest">Uppdaterad information</param>
    /// <returns>Den uppdaterade boken</returns>
    /// <response code="200">Boken har uppdaterats</response>
    /// <response code="404">Boken hittades inte</response>
    /// <response code="400">Ogiltig data</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookRequest updateBookRequest)
    {
        updateBookRequest.Id = id;
        var response = await _grpcClient.UpdateBookAsync(updateBookRequest);
        
        if (!response.Found)
            return NotFound($"Bok med ID {id} hittades inte.");
        
        return Ok(response.Book);
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
        var request = new DeleteBookRequest { Id = id };
        var response = await _grpcClient.DeleteBookAsync(request);
        
        if (!response.Success)
            return NotFound($"Bok med ID {id} hittades inte.");
        
        return NoContent();
    }
}
