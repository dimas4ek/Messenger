using Domain.Entities;

namespace Application.Interfaces;

public interface IFriendRequestRepository : IRepository<FriendRequest>
{
    Task<List<FriendRequest>> GetRequestsByUserId(int userId);
    Task<FriendRequest?> GetByIdWithUsers(int requestId);
}