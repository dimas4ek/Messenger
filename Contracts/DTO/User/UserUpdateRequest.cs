using System.Text.Json.Serialization;
using Application.DTO;

namespace Contracts.DTO.User;

public class UserUpdateRequest
{
    [JsonPropertyName("username")] public string? Username { get; set; }

    [JsonPropertyName("password")] public string? Password { get; set; }

    [JsonPropertyName("avatar")] public ImageInfo? Avatar { get; set; }
}