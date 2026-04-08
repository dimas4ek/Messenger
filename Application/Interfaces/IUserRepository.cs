using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsername(string username);
    Task<bool> UserExists(string username);
    Task<List<User>> SearchByUsername(string text);
}