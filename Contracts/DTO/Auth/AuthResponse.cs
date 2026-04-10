using System.Text.Json.Serialization;
using Application.DTO;

namespace Contracts.DTO.Auth;

public class AuthResponse
{
    [JsonPropertyName("user")] public UserInfo? User { get; set; }
}