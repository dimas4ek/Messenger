using Application.DTO;

namespace Contracts.DTO.Auth;

public class AuthResponse
{
    public UserInfo? User { get; set; }
}