using System.Net.Http.Json;
using Application.Utils;
using Client.Utils;
using Contracts.DTO;
using Contracts.DTO.Chat;

namespace Client.ApiClients;

public class ChatApiClient(HttpClient httpClient)
{
    public async Task<ApiResult<ChatResponse>> LoadPrivateChat(int currentUserId, int companionId)
    {
        try
        {
            var response =
                await httpClient.GetAsync($"api/chat/private?userId={currentUserId}&companionId={companionId}");

            if (response.IsSuccessStatusCode)
            {
                var chatResponse = await response.Content.ReadFromJsonAsync<ChatResponse>();

                return chatResponse == null
                    ? ApiResult<ChatResponse>.Failure(ErrorCode.EmptyResponse)
                    : ApiResult<ChatResponse>.Success(chatResponse);
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return ApiResult<ChatResponse>.Failure(errorResponse?.ErrorCode ?? ErrorCode.UnknownError);
        }
        catch
        {
            return ApiResult<ChatResponse>.Failure(ErrorCode.DatabaseError);
        }
    }

    public async Task<ApiResult<MessageResponse>> SendMessage(int currentUserId, int companionId, string message)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("api/chat/message", new SendMessageRequest
            {
                SenderId = currentUserId,
                CompanionId = companionId,
                Message = message
            });

            if (response.IsSuccessStatusCode)
            {
                var messageResponse = await response.Content.ReadFromJsonAsync<MessageResponse>();

                return messageResponse == null
                    ? ApiResult<MessageResponse>.Failure(ErrorCode.EmptyResponse)
                    : ApiResult<MessageResponse>.Success(messageResponse);
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return ApiResult<MessageResponse>.Failure(errorResponse?.ErrorCode ?? ErrorCode.UnknownError);
        }
        catch
        {
            return ApiResult<MessageResponse>.Failure(ErrorCode.DatabaseError);
        }
    }
}