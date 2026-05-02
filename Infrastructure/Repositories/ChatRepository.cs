using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ChatRepository(MessengerContext context) : Repository<Chat>(context), IChatRepository
{
    private readonly MessengerContext _context = context;

    public async Task<Chat?> GetChatById(int chatId)
    {
        return await _context.Chats
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(m => m.Id == chatId);
    }

    public async Task<List<Chat>> GetChatsByUserId(int userId)
    {
        return await _context.Chats
            .Where(c => c.Participants.Any(cp => cp.ParticipantId == userId))
            .Select(c => new Chat
            {
                Id = c.Id,
                Type = c.Type,
                Name = c.Type == ChatType.Private
                    ? c.Participants.FirstOrDefault(cp => cp.ParticipantId != userId).Participant.Username
                    : c.Name,
                Image = c.Type == ChatType.Private
                    ? c.Participants.FirstOrDefault(cp => cp.ParticipantId != userId).Participant.Avatar
                    : c.Image,
                Participants = c.Participants.Select(cp => new ChatParticipant
                {
                    ChatId = cp.ChatId,
                    ParticipantId = cp.ParticipantId,
                    Participant = new User
                    {
                        Username = cp.Participant.Username,
                        Status = cp.Participant.Status
                    },
                    Role = cp.Role
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<Chat?> GetChat(int chatId, int userId)
    {
        return await _context.Chats
            .Include(c => c.Messages).ThenInclude(m => m.Sender)
            .Include(c => c.Participants).ThenInclude(p => p.Participant).ThenInclude(u => u.Avatar)
            .Include(c => c.Image)
            .Where(c => c.Id == chatId)
            .Select(c => new Chat
            {
                Id = c.Id,
                Type = c.Type,
                Name = c.Type == ChatType.Private
                    ? c.Participants.FirstOrDefault(p => p.ParticipantId != userId).Participant.Username
                    : c.Name,
                Image = c.Type == ChatType.Private
                    ? c.Participants.FirstOrDefault(p => p.ParticipantId != userId).Participant.Avatar
                    : c.Image,
                Messages = c.Messages,
                Participants = c.Participants
            })
            .FirstOrDefaultAsync();
    }

    public async Task AddParticipants(IEnumerable<ChatParticipant> participants)
    {
        await _context.AddRangeAsync(participants);
    }

    public async Task<List<ChatParticipant>> GetParticipants(int chatId)
    {
        return await _context.ChatParticipants
            .Include(cp => cp.Participant)
            .ThenInclude(u => u.Avatar)
            .Where(cp => cp.ChatId == chatId)
            .ToListAsync();
    }
}