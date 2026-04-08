using Domain.Entities;

namespace Application.Interfaces;

public interface IConversationRepository : IRepository<Conversation>
{
    Task<Conversation?> GetByUsersId(int userId, int companionId);
    Task AddParticipants(IEnumerable<ConversationParticipant> participants);
    Task<List<ConversationParticipant>> GetParticipants(int conversationId);
}