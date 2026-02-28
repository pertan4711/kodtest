using Microsoft.AspNetCore.Mvc;
using LibraryService.Grpc.Library;

namespace uppgift2.api.Controllers;

/// <summary>
/// Statistik och analyser för biblioteket
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LibraryController : ControllerBase
{
    private readonly LibraryService.Grpc.Library.LibraryService.LibraryServiceClient _grpcClient;

    public LibraryController(LibraryService.Grpc.Library.LibraryService.LibraryServiceClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    /// <summary>
    /// Vilka är de mest lånade böckerna?
    /// </summary>
    /// <param name="top">Antal böcker att returnera (standard: 10)</param>
    /// <returns>Lista över de mest lånade böckerna med antal lån</returns>
    /// <response code="200">Returnerar listan över mest lånade böcker</response>
    [HttpGet("most-borrowed")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMostBorrowedBooks([FromQuery] int top = 10)
    {
        var request = new MostBorrowedRequest { Top = top };
        var response = await _grpcClient.GetMostBorrowedBooksAsync(request);
        
        return Ok(response.Books);
    }

    /// <summary>
    /// Hur många exemplar av en viss bok är för närvarande utlånade respektive tillgängliga?
    /// </summary>
    /// <param name="bookId">Bokens ID</param>
    /// <returns>Information om bokens tillgänglighet</returns>
    /// <response code="200">Returnerar tillgänglighetsinformation</response>
    /// <response code="404">Boken hittades inte</response>
    [HttpGet("books/{bookId}/availability")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBookAvailability(int bookId)
    {
        var request = new BookAvailabilityRequest { BookId = bookId };
        var response = await _grpcClient.GetBookAvailabilityAsync(request);
        
        if (!response.Found)
            return NotFound($"Bok med ID {bookId} hittades inte.");
        
        return Ok(response);
    }

    /// <summary>
    /// Vilka användare har lånat flest böcker under en viss tidsperiod?
    /// </summary>
    /// <param name="startDate">Startdatum (format: yyyy-MM-dd)</param>
    /// <param name="endDate">Slutdatum (format: yyyy-MM-dd)</param>
    /// <param name="top">Antal användare att returnera (standard: 10)</param>
    /// <returns>Lista över mest aktiva låntagare</returns>
    /// <response code="200">Returnerar listan över mest aktiva låntagare</response>
    /// <response code="400">Ogiltiga datum</response>
    [HttpGet("top-borrowers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTopBorrowers(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate, 
        [FromQuery] int top = 10)
    {
        if (startDate > endDate)
            return BadRequest("Startdatum kan inte vara senare än slutdatum.");

        var request = new TopBorrowersRequest 
        { 
            StartDate = startDate.ToString("yyyy-MM-dd"),
            EndDate = endDate.ToString("yyyy-MM-dd"),
            Top = top
        };
        var response = await _grpcClient.GetTopBorrowersAsync(request);
        
        return Ok(response.Borrowers);
    }

    /// <summary>
    /// Vilka böcker har en enskild användare lånat under respektive tidsperiod?
    /// </summary>
    /// <param name="userId">Användarens ID</param>
    /// <param name="startDate">Startdatum (valfritt, format: yyyy-MM-dd)</param>
    /// <param name="endDate">Slutdatum (valfritt, format: yyyy-MM-dd)</param>
    /// <returns>Användarens lånehistorik</returns>
    /// <response code="200">Returnerar användarens lånehistorik</response>
    [HttpGet("users/{userId}/loan-history")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserLoanHistory(
        int userId, 
        [FromQuery] DateTime? startDate = null, 
        [FromQuery] DateTime? endDate = null)
    {
        var request = new UserLoanHistoryRequest 
        { 
            UserId = userId
        };
        
        if (startDate.HasValue)
            request.StartDate = startDate.Value.ToString("yyyy-MM-dd");
        if (endDate.HasValue)
            request.EndDate = endDate.Value.ToString("yyyy-MM-dd");

        var response = await _grpcClient.GetUserLoanHistoryAsync(request);
        
        return Ok(response.Loans);
    }

    /// <summary>
    /// Vilka andra böcker har lånats av personer som lånat en viss bok?
    /// </summary>
    /// <param name="bookId">Bokens ID</param>
    /// <param name="top">Antal relaterade böcker att returnera (standard: 10)</param>
    /// <returns>Lista över relaterade böcker baserat på gemensamma låntagare</returns>
    /// <response code="200">Returnerar listan över relaterade böcker</response>
    [HttpGet("books/{bookId}/related")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRelatedBooks(int bookId, [FromQuery] int top = 10)
    {
        var request = new RelatedBooksRequest 
        { 
            BookId = bookId,
            Top = top
        };
        var response = await _grpcClient.GetRelatedBooksAsync(request);
        
        return Ok(response.Books);
    }

    /// <summary>
    /// Ungefärlig läshastighet för en viss bok, uttryckt i sidor per dag
    /// </summary>
    /// <param name="bookId">Bokens ID</param>
    /// <returns>Genomsnittlig läshastighet baserat på låneperioder</returns>
    /// <response code="200">Returnerar läshastighetsinformation</response>
    /// <response code="404">Boken hittades inte</response>
    /// <remarks>
    /// Beräkningen utgår från antagandet att användare börjar läsa direkt vid utlåning 
    /// och lämnar tillbaka boken så snart de läst klart.
    /// </remarks>
    [HttpGet("books/{bookId}/reading-speed")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReadingSpeed(int bookId)
    {
        var request = new ReadingSpeedRequest { BookId = bookId };
        var response = await _grpcClient.GetReadingSpeedAsync(request);
        
        if (!response.Found)
            return NotFound($"Bok med ID {bookId} hittades inte.");
        
        return Ok(response);
    }
}
