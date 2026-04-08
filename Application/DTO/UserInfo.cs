using Domain.Enums;

namespace Application.DTO;

public class UserInfo
{
    public UserInfo()
    {
    }

    public UserInfo(int id, string username, UserStatus status = UserStatus.Offline)
    {
        Id = id;
        Username = username;
        Status = status;
    }

    public int Id { get; set; }
    public required string Username { get; set; }
    public UserStatus Status { get; set; }
}