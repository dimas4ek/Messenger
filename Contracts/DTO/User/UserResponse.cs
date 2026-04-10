using System.Text.Json.Serialization;
using Application.DTO;

namespace Contracts.DTO.User;

public class UserResponse
{
    [JsonPropertyName("user")] public UserInfo User { get; set; }
}