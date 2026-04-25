using System.Net.Http.Json;
using Application.Utils;
using Client.Utils;
using Contracts.DTO;
using Contracts.DTO.Chat;
using Contracts.DTO.Image;
using Domain.Enums;

namespace Client.Api.Clients;

public class ChatApiClient(HttpClient httpClient)
{
    public async Task<ApiResult<ChatListResponse>> GetChatList(int currentUserId)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/chat/list?userId={currentUserId}");

            if (response.IsSuccessStatusCode)
            {
                var chatListResponse = await response.Content.ReadFromJsonAsync<ChatListResponse>();

                return chatListResponse == null
                    ? ApiResult<ChatListResponse>.Failure(ErrorCode.EmptyResponse)
                    : ApiResult<ChatListResponse>.Success(chatListResponse);
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return ApiResult<ChatListResponse>.Failure(errorResponse?.ErrorCode ?? ErrorCode.UnknownError);
        }
        catch
        {
            return ApiResult<ChatListResponse>.Failure(ErrorCode.DatabaseError);
        }
    }

    public async Task<ApiResult<ChatResponse>> CreateGroupChat(string name, int? imageId, int currentUserId,
        IEnumerable<int> addedUserIds)
    {
        try
        {
            var response =
                await httpClient.PostAsJsonAsync("api/chat/group", new GroupChatRequest
                {
                    Name = name,
                    ImageId = imageId,
                    CreatorId = currentUserId,
                    AddedUserIds = addedUserIds
                });

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

    public async Task<ApiResult<ChatResponse>> LoadChat(int currentUserId, int chatId)
    {
        try
        {
            var response =
                await httpClient.GetAsync(
                    $"api/chat/{chatId}?userId={currentUserId}");

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
            var response = await httpClient.PostAsJsonAsync($"api/chat/{currentChatId}/messages", new SendMessageRequest
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
            var response = await httpClient.PatchAsJsonAsync($"api/chat/{chatId}/messages/{messageId}",
                new EditMessageRequest
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
            var response = await httpClient.DeleteAsync($"api/chat/{chatId}/messages/{messageId}");

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

    public async Task<ApiResult<ImageResponse>> ChangeGroupImage(int chatId, string name, byte[] imageBytes,
        ImageContentType contentType)
    {
        try
        {
            var response = await httpClient.PatchAsJsonAsync($"api/chat/{chatId}/image", new ImageRequest
            {
                Name = name,
                Bytes = imageBytes,
                ContentType = contentType
            });

            if (response.IsSuccessStatusCode)
            {
                var imageResponse = await response.Content.ReadFromJsonAsync<ImageResponse>();

                return imageResponse == null
                    ? ApiResult<ImageResponse>.Failure(ErrorCode.EmptyResponse)
                    : ApiResult<ImageResponse>.Success(imageResponse);
            }

            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return ApiResult<ImageResponse>.Failure(errorResponse?.ErrorCode ?? ErrorCode.UnknownError);
        }
        catch
        {
            return ApiResult<ImageResponse>.Failure(ErrorCode.DatabaseError);
        }
    }
}