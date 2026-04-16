using System.Text.Json.Serialization;

namespace Contracts.DTO.User;

public class UserUpdateRequest
{
    [JsonPropertyName("username")] public string? Username { get; set; }

    [JsonPropertyName("password")] public string? Password { get; set; }
}