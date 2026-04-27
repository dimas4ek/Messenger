using System.Text.Json.Serialization;

namespace Contracts.DTO.User;

public class UserUpdateRequest
{
    [JsonPropertyName("username")] public string? Username { get; init; }

    [JsonPropertyName("password")] public string? Password { get; init; }
}