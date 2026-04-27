using System.Text.Json.Serialization;

namespace Contracts.DTO.Auth;

public class LogoutResponse
{
    [JsonPropertyName("success")] public bool Success { get; init; }
}