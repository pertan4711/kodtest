using Microsoft.AspNetCore.Mvc;
using LibraryService.Grpc.Users;

namespace uppgift2.api.Controllers;

/// <summary>
/// CRUD-operationer för användare
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService.UserServiceClient _grpcClient;

    public UsersController(UserService.UserServiceClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    /// <summary>
    /// Hämtar alla användare
    /// </summary>
    /// <returns>Lista över alla användare i systemet</returns>
    /// <response code="200">Returnerar listan över alla användare</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers()
    {
        var request = new GetAllUsersRequest();
        var response = await _grpcClient.GetAllUsersAsync(request);
        
        return Ok(response.Users);
    }

    /// <summary>
    /// Hämtar en specifik användare
    /// </summary>
    /// <param name="id">Användarens ID</param>
    /// <returns>Användaren med angivet ID</returns>
    /// <response code="200">Returnerar användaren</response>
    /// <response code="404">Användaren hittades inte</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(int id)
    {
        var request = new GetUserRequest { Id = id };
        var response = await _grpcClient.GetUserAsync(request);
        
        if (!response.Found)
            return NotFound($"Användare med ID {id} hittades inte.");
        
        return Ok(response.User);
    }

    /// <summary>
    /// Skapar en ny användare
    /// </summary>
    /// <param name="createUserRequest">Användarens information</param>
    /// <returns>Den nyskapade användaren</returns>
    /// <response code="201">Användaren har skapats</response>
    /// <response code="400">Ogiltig data</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest createUserRequest)
    {
        var response = await _grpcClient.CreateUserAsync(createUserRequest);
        
        return CreatedAtAction(nameof(GetUser), new { id = response.User.Id }, response.User);
    }

    /// <summary>
    /// Uppdaterar en befintlig användare
    /// </summary>
    /// <param name="id">Användarens ID</param>
    /// <param name="updateUserRequest">Uppdaterad information</param>
    /// <returns>Den uppdaterade användaren</returns>
    /// <response code="200">Användaren har uppdaterats</response>
    /// <response code="404">Användaren hittades inte</response>
    /// <response code="400">Ogiltig data</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest updateUserRequest)
    {
        updateUserRequest.Id = id;
        var response = await _grpcClient.UpdateUserAsync(updateUserRequest);
        
        if (!response.Found)
            return NotFound($"Användare med ID {id} hittades inte.");
        
        return Ok(response.User);
    }

    /// <summary>
    /// Tar bort en användare
    /// </summary>
    /// <param name="id">Användarens ID</param>
    /// <returns>Inget innehåll</returns>
    /// <response code="204">Användaren har tagits bort</response>
    /// <response code="404">Användaren hittades inte</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var request = new DeleteUserRequest { Id = id };
        var response = await _grpcClient.DeleteUserAsync(request);
        
        if (!response.Success)
            return NotFound($"Användare med ID {id} hittades inte.");
        
        return NoContent();
    }
}
