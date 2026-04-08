using Domain.Entities;

namespace Application.Interfaces;

public interface IMessageRepository : IRepository<Message>
{
    Task<List<Message>> GetConversationMessages(int conversationId);
    Task<int?> GetSenderId(int messageId);
    Task<Message?> GetMessageById(int messageId);
}