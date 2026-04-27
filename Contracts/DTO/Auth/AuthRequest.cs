using System.Text.Json.Serialization;

namespace Contracts.DTO.Auth;

public class AuthRequest
{
    [JsonPropertyName("username")] public string Username { get; init; } = string.Empty;

    [JsonPropertyName("password")] public string Password { get; init; } = string.Empty;
}