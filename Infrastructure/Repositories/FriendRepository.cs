using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FriendRepository(MessengerContext context)
    : Repository<Friendship>(context), IFriendRepository
{
    private readonly MessengerContext _context = context;

    public async Task AddFriend(int userId, int friendId)
    {
        await _context.Friends.AddRangeAsync(new Friendship { UserId = userId, FriendId = friendId },
            new Friendship { UserId = friendId, FriendId = userId });
    }

    public async Task<List<Friendship>> GetFriendsByUserId(int userId)
    {
        return await _context.Friends
            .Include(f => f.Friend)
            .Where(f => f.UserId == userId)
            .ToListAsync();
    }

    public Task<bool> IsFriends(int userId, int friendId)
    {
        return _context.Friends
            .AnyAsync(fl => fl.UserId == userId && fl.FriendId == friendId);
    }
}