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
                await httpClient.GetAsync($"api/chats/private?userId={currentUserId}&companionId={companionId}");

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

    public async Task<ApiResult<MessageResponse>> SendMessage(int currentChatId, int senderId, string message)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync($"api/chats/{currentChatId}/messages", new SendMessageRequest
            {
                SenderId = senderId,
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

    public async Task<ApiResult<MessageResponse>> EditMessage(int chatId, int messageId, string newText)
    {
        try
        {
            var response = await httpClient.PatchAsJsonAsync($"api/chats/{chatId}/messages/{messageId}", new EditMessageRequest
            {
                Message = newText
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

    public async Task<ApiResult<DeleteResponse>> DeleteMessage(int chatId, int messageId)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"api/chats/{chatId}/messages/{messageId}");

            if (response.IsSuccessStatusCode)
            {
                var deleteResponse = await response.Content.ReadFromJsonAsync<DeleteResponse>();

                return deleteResponse == null
                    ? ApiResult<DeleteResponse>.Failure(ErrorCode.EmptyResponse)
                    : ApiResult<DeleteResponse>.Success(deleteResponse);
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return ApiResult<DeleteResponse>.Failure(errorResponse?.ErrorCode ?? ErrorCode.UnknownError);
        }
        catch
        {
            return ApiResult<DeleteResponse>.Failure(ErrorCode.DatabaseError);
        }
    }
}