using Microsoft.EntityFrameworkCore;
using uppgift2.service.Data;
using uppgift2.service.Models;
using LibraryService.Interfaces;

namespace uppgift2.service.Services;

public class UserService : IUserService
{
    private readonly LibraryContext _context;

    public UserService(LibraryContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .Include(u => u.Loans)
            .ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Loans)
            .ThenInclude(l => l.BookCopy)
            .ThenInclude(bc => bc.Book)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        return user;
    }

    public async Task<User?> UpdateUserAsync(int id, User user)
    {
        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null)
            return null;

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.MemberSince = user.MemberSince;

        await _context.SaveChangesAsync();
        
        return existingUser;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        
        return true;
    }
}
