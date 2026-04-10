using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ChatRepository(MessengerContext context) : Repository<Chat>(context), IChatRepository
{
    private readonly MessengerContext _context = context;

    public async Task<Chat?> GetByParticipantsId(int userId, int companionId)
    {
        return await _context.Chats
            .Include(c => c.Messages).ThenInclude(m => m.Sender)
            .Include(c => c.Participants).ThenInclude(p => p.Participant)
            .Where(c => c.Type == ChatType.Private)
            .Where(c => c.Participants.Count == 2)
            .Where(c => c.Participants.Any(p => p.ParticipantId == userId))
            .Where(c => c.Participants.Any(p => p.ParticipantId == companionId))
            .FirstOrDefaultAsync();
    }

    public async Task AddParticipants(IEnumerable<ChatParticipant> participants)
    {
        await _context.AddRangeAsync(participants);
    }

    public async Task<List<ChatParticipant>> GetParticipants(int chatId)
    {
        return await _context.ChatParticipants
            .Where(cp => cp.ChatId == chatId)
            .ToListAsync();
    }
}