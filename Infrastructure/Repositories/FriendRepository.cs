using Application.DTO;
using Application.Interfaces;
using Application.Utils.Mapper;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FriendRepository : Repository<Friendship>, IFriendRepository
{
    private readonly MessengerContext _context;
    private readonly IAppMapper _mapper;

    public FriendRepository(MessengerContext context, IAppMapper mapper) : base(context)
    {
        _context = context;
        _mapper = mapper;
    }

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

    public async Task<List<UserInfo>> SearchFriends(int userId, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return [];

        return await _context.Friends
            .Where(f =>
                f.UserId == userId &&
                EF.Functions.ILike(f.Friend.Username, $"%{text}%"))
            .Select(f => _mapper.Map<User, UserInfo>(f.Friend))
            .ToListAsync();
    }

    public Task<bool> IsFriends(int userId, int friendId)
    {
        return _context.Friends
            .AnyAsync(fl => fl.UserId == userId && fl.FriendId == friendId);
    }
}