using System.Text.Json.Serialization;

namespace Contracts.DTO.Auth;

public class LogoutRequest
{
    [JsonPropertyName("userId")] public int UserId { get; init; }
}