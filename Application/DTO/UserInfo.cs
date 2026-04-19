using Domain.Enums;

namespace Application.DTO;

public class UserInfo
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public UserStatus Status { get; set; }
}