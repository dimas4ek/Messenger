using Domain.Entities;

namespace Application.Interfaces;

public interface IChatRepository : IRepository<Chat>
{
    Task<Chat?> GetChatById(int chatId);
    Task<List<Chat>> GetChatsByUserId(int userId);
    Task<Chat?> GetChat(int chatId, int userId);
    Task AddParticipants(IEnumerable<ChatParticipant> participants);
    Task<List<ChatParticipant>> GetParticipants(int chatId);
}