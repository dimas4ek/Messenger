using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ConversationRepository : Repository<Conversation>, IConversationRepository
{
    private readonly MessengerContext _context;

    public ConversationRepository(MessengerContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Conversation?> GetByUsersId(int userId, int companionId)
    {
        return await _context.Conversations
            .Include(c => c.Messages).ThenInclude(m => m.Sender)
            .Include(c => c.Participants).ThenInclude(p => p.Participant)
            .Where(c => c.Type == ConversationType.Private)
            .Where(c => c.Participants.Count == 2)
            .Where(c => c.Participants.Any(p => p.ParticipantId == userId))
            .Where(c => c.Participants.Any(p => p.ParticipantId == companionId))
            .FirstOrDefaultAsync();
    }

    public async Task AddParticipants(IEnumerable<ConversationParticipant> participants)
    {
        await _context.AddRangeAsync(participants);
    }

    public async Task<List<ConversationParticipant>> GetParticipants(int conversationId)
    {
        return await _context.ConversationParticipants
            .Where(cp => cp.ConversationId == conversationId)
            .ToListAsync();
    }
}