using System.Text.Json.Serialization;
using Application.DTO;

namespace Contracts.DTO.User;

public class UserResponse
{
    [JsonPropertyName("user")] public required UserInfo User { get; init; }
}