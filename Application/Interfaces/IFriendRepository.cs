using Application.DTO;
using Domain.Entities;

namespace Application.Interfaces;

public interface IFriendRepository : IRepository<Friendship>
{
    Task AddFriend(int userId, int friendId);
    Task<List<Friendship>> GetFriendsByUserId(int userId);
    Task<List<UserInfo>> SearchFriends(int userId, string text);
    Task<bool> IsFriends(int userId, int friendId);
}