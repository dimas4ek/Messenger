using System.Text.Json.Serialization;
using Application.Utils;

namespace Contracts.DTO;

public class ErrorResponse
{
    [JsonPropertyName("errorCode")] public ErrorCode ErrorCode { get; set; }
}