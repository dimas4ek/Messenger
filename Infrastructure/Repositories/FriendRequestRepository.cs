using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FriendRequestRepository(MessengerContext context)
    : Repository<FriendRequest>(context), IFriendRequestRepository
{
    private readonly MessengerContext _context = context;

    public Task<List<FriendRequest>> GetRequestsByUserId(int userId)
    {
        return _context.FriendRequests
            .Include(fr => fr.Sender)
            .Where(fr => fr.ReceiverId == userId)
            .OrderBy(fr => fr.CreatedAt)
            .ToListAsync();
    }

    public async Task<FriendRequest?> GetByIdWithUsers(int requestId)
    {
        return await _context.FriendRequests
            .Include(fr => fr.Sender)
            .Include(fr => fr.Receiver)
            .FirstOrDefaultAsync(fr => fr.Id == requestId);
    }
}