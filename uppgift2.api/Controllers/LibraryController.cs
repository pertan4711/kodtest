using Microsoft.AspNetCore.Mvc;
using uppgift2.service.Services;

namespace uppgift2.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibraryController : ControllerBase
{
    private readonly ILibraryService _libraryService;

    public LibraryController(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    /// <summary>
    /// Hämtar de mest lånade böckerna
    /// </summary>
    /// <param name="top">Antal böcker att returnera (standard: 10)</param>
    [HttpGet("most-borrowed")]
    public async Task<IActionResult> GetMostBorrowedBooks([FromQuery] int top = 10)
    {
        var result = await _libraryService.GetMostBorrowedBooksAsync(top);
        return Ok(result);
    }

    /// <summary>
    /// Hämtar tillgänglighet för en specifik bok
    /// </summary>
    /// <param name="bookId">Bokens ID</param>
    [HttpGet("books/{bookId}/availability")]
    public async Task<IActionResult> GetBookAvailability(int bookId)
    {
        var result = await _libraryService.GetBookAvailabilityAsync(bookId);
        
        if (result == null)
            return NotFound($"Bok med ID {bookId} hittades inte.");
        
        return Ok(result);
    }

    /// <summary>
    /// Hämtar användare som lånat flest böcker under en tidsperiod
    /// </summary>
    /// <param name="startDate">Startdatum (format: yyyy-MM-dd)</param>
    /// <param name="endDate">Slutdatum (format: yyyy-MM-dd)</param>
    /// <param name="top">Antal användare att returnera (standard: 10)</param>
    [HttpGet("top-borrowers")]
    public async Task<IActionResult> GetTopBorrowers(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate, 
        [FromQuery] int top = 10)
    {
        if (startDate > endDate)
            return BadRequest("Startdatum kan inte vara senare än slutdatum.");

        var result = await _libraryService.GetTopBorrowersAsync(startDate, endDate, top);
        return Ok(result);
    }

    /// <summary>
    /// Hämtar lånehistorik för en användare
    /// </summary>
    /// <param name="userId">Användarens ID</param>
    /// <param name="startDate">Startdatum (valfritt, format: yyyy-MM-dd)</param>
    /// <param name="endDate">Slutdatum (valfritt, format: yyyy-MM-dd)</param>
    [HttpGet("users/{userId}/loan-history")]
    public async Task<IActionResult> GetUserLoanHistory(
        int userId, 
        [FromQuery] DateTime? startDate = null, 
        [FromQuery] DateTime? endDate = null)
    {
        var result = await _libraryService.GetUserLoanHistoryAsync(userId, startDate, endDate);
        return Ok(result);
    }

    /// <summary>
    /// Hämtar relaterade böcker baserat på vad andra låntagare av en bok har lånat
    /// </summary>
    /// <param name="bookId">Bokens ID</param>
    /// <param name="top">Antal relaterade böcker att returnera (standard: 10)</param>
    [HttpGet("books/{bookId}/related")]
    public async Task<IActionResult> GetRelatedBooks(int bookId, [FromQuery] int top = 10)
    {
        var result = await _libraryService.GetRelatedBooksAsync(bookId, top);
        return Ok(result);
    }

    /// <summary>
    /// Beräknar genomsnittlig läshastighet för en bok i sidor per dag
    /// </summary>
    /// <param name="bookId">Bokens ID</param>
    [HttpGet("books/{bookId}/reading-speed")]
    public async Task<IActionResult> GetReadingSpeed(int bookId)
    {
        var result = await _libraryService.GetReadingSpeedAsync(bookId);
        
        if (result == null)
            return NotFound($"Bok med ID {bookId} hittades inte.");
        
        return Ok(result);
    }
}
