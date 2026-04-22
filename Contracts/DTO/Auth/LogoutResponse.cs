using System.Text.Json.Serialization;
using Application.DTO;

namespace Contracts.DTO.Auth;

public class LogoutResponse
{
    [JsonPropertyName("success")] public UserInfo User { get; set; }
}