using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MessageRepository : Repository<Message>, IMessageRepository
{
    private readonly MessengerContext _context;

    public MessageRepository(MessengerContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Message>> GetConversationMessages(int conversationId)
    {
        return await _context.Messages
            .Include(m => m.Sender)
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<int?> GetSenderId(int messageId)
    {
        return await _context.Messages
            .Where(m => m.Id == messageId)
            .Select(m => (int?)m.SenderId)
            .FirstOrDefaultAsync();
    }

    public async Task<Message?> GetMessageById(int messageId)
    {
        return await _context.Messages
            .Include(m => m.Sender)
            .FirstOrDefaultAsync(m => m.Id == messageId);
    }
}