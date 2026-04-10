using Domain.Entities;

namespace Application.Interfaces;

public interface IChatRepository : IRepository<Chat>
{
    Task<Chat?> GetByParticipantsId(int userId, int companionId);
    Task AddParticipants(IEnumerable<ChatParticipant> participants);
    Task<List<ChatParticipant>> GetParticipants(int chatId);
}