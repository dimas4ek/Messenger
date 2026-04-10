using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(MessengerContext context) : Repository<User>(context), IUserRepository
{
    private readonly MessengerContext _context = context;

    public async Task<User?> GetByUsername(string username)
    {
        return await _context.Users
            .Where(u => u.Username == username)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }
}