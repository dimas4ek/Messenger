using Domain.Entities;

namespace Application.Interfaces;

public interface IMessageRepository : IRepository<Message>
{
    Task<List<Message>> GetChatMessages(int chatId);
    Task<int?> GetSenderId(int messageId);
    Task<Message?> GetMessageById(int messageId);
}