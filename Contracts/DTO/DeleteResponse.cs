using System.Text.Json.Serialization;

namespace Contracts.DTO;

public class DeleteResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }
}