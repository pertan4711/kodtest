using Microsoft.AspNetCore.Mvc;
using uppgift2.service.DTOs;
using uppgift2.service.Models;
using uppgift2.service.Services;

namespace uppgift2.api.Controllers;

/// <summary>
/// CRUD-operationer för användare
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
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
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
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
        var user = await _userService.GetUserByIdAsync(id);
        
        if (user == null)
            return NotFound($"Användare med ID {id} hittades inte.");
        
        return Ok(user);
    }

    /// <summary>
    /// Skapar en ny användare
    /// </summary>
    /// <param name="createUserDto">Användarens information</param>
    /// <returns>Den nyskapade användaren</returns>
    /// <response code="201">Användaren har skapats</response>
    /// <response code="400">Ogiltig data</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        var user = new User
        {
            Name = createUserDto.Name,
            Email = createUserDto.Email,
            MemberSince = createUserDto.MemberSince
        };

        var createdUser = await _userService.CreateUserAsync(user);
        return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
    }

    /// <summary>
    /// Uppdaterar en befintlig användare
    /// </summary>
    /// <param name="id">Användarens ID</param>
    /// <param name="updateUserDto">Uppdaterad information</param>
    /// <returns>Den uppdaterade användaren</returns>
    /// <response code="200">Användaren har uppdaterats</response>
    /// <response code="404">Användaren hittades inte</response>
    /// <response code="400">Ogiltig data</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
    {
        var user = new User
        {
            Name = updateUserDto.Name,
            Email = updateUserDto.Email,
            MemberSince = updateUserDto.MemberSince
        };

        var updatedUser = await _userService.UpdateUserAsync(id, user);
        
        if (updatedUser == null)
            return NotFound($"Användare med ID {id} hittades inte.");
        
        return Ok(updatedUser);
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
        var result = await _userService.DeleteUserAsync(id);
        
        if (!result)
            return NotFound($"Användare med ID {id} hittades inte.");
        
        return NoContent();
    }
}
