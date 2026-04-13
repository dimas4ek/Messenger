using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MessageRepository(MessengerContext context) : Repository<Message>(context), IMessageRepository
{
    private readonly MessengerContext _context = context;

    public async Task<List<Message>> GetChatMessages(int chatId)
    {
        return await _context.Messages
            .Include(m => m.Sender)
            .Where(m => m.ChatId == chatId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<Message?> GetMessageById(int messageId)
    {
        return await _context.Messages
            .Include(m => m.Sender)
            .FirstOrDefaultAsync(m => m.Id == messageId);
    }
}