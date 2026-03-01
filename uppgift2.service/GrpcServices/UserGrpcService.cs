using Grpc.Core;
using LibraryService.Grpc.Users;
using LibraryService.Interfaces;
using uppgift2.service.Models;
using uppgift2.service.Services;

namespace uppgift2.service.GrpcServices;

public class UserGrpcService : LibraryService.Grpc.Users.UserService.UserServiceBase
{
    private readonly IUserService _userService;

    public UserGrpcService(IUserService userService)
    {
        _userService = userService;
    }

    public override async Task<GetAllUsersResponse> GetAllUsers(
        GetAllUsersRequest request,
        ServerCallContext context)
    {
        var users = await _userService.GetAllUsersAsync();

        var response = new GetAllUsersResponse();
        foreach (var user in users)
        {
            response.Users.Add(MapToUserDto(user));
        }

        return response;
    }

    public override async Task<GetUserResponse> GetUser(
        GetUserRequest request,
        ServerCallContext context)
    {
        var user = await _userService.GetUserByIdAsync(request.Id);

        if (user == null)
        {
            return new GetUserResponse { Found = false };
        }

        return new GetUserResponse
        {
            User = MapToUserDto(user),
            Found = true
        };
    }

    public override async Task<CreateUserResponse> CreateUser(
        CreateUserRequest request,
        ServerCallContext context)
    {
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            MemberSince = DateTime.Parse(request.MemberSince)
        };

        var createdUser = await _userService.CreateUserAsync(user);

        return new CreateUserResponse
        {
            User = MapToUserDto(createdUser)
        };
    }

    public override async Task<UpdateUserResponse> UpdateUser(
        UpdateUserRequest request,
        ServerCallContext context)
    {
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            MemberSince = DateTime.Parse(request.MemberSince)
        };

        var updatedUser = await _userService.UpdateUserAsync(request.Id, user);

        if (updatedUser == null)
        {
            return new UpdateUserResponse { Found = false };
        }

        return new UpdateUserResponse
        {
            User = MapToUserDto(updatedUser),
            Found = true
        };
    }

    public override async Task<DeleteUserResponse> DeleteUser(
        DeleteUserRequest request,
        ServerCallContext context)
    {
        var success = await _userService.DeleteUserAsync(request.Id);

        return new DeleteUserResponse { Success = success };
    }

    private UserDto MapToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            MemberSince = user.MemberSince.ToString("yyyy-MM-dd")
        };
    }
}
