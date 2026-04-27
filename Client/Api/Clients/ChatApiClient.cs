using Client.Utils;
using Contracts.DTO;
using Contracts.DTO.Chat;
using Contracts.DTO.Image;
using Domain.Enums;

namespace Client.Api.Clients;

public class ChatApiClient(HttpClient httpClient) : ApiClientBase(httpClient)
{
    public Task<ApiResult<ChatListResponse>> GetChatList(int currentUserId)
    {
        return GetAsync<ChatListResponse>($"api/chat/list?userId={currentUserId}");
    }

    public Task<ApiResult<ChatResponse>> CreateGroupChat(string name, int? imageId, int currentUserId,
        IEnumerable<int> addedUserIds)
    {
        return PostAsync<ChatResponse>("api/chat/group", new GroupChatRequest
        {
            Name = name,
            ImageId = imageId,
            CreatorId = currentUserId,
            AddedUserIds = addedUserIds
        });
    }

    public Task<ApiResult<ChatResponse>> LoadChat(int currentUserId, int chatId)
    {
        return GetAsync<ChatResponse>($"api/chat/{chatId}?userId={currentUserId}");
    }

    public Task<ApiResult<MessageResponse>> SendMessage(int currentChatId, int senderId, string message)
    {
        return PostAsync<MessageResponse>($"api/chat/{currentChatId}/messages", new SendMessageRequest
        {
            SenderId = senderId,
            Message = message
        });
    }

    public Task<ApiResult<MessageResponse>> EditMessage(int chatId, int messageId, string newText)
    {
        return PatchAsync<MessageResponse>($"api/chat/{chatId}/messages/{messageId}",
            new EditMessageRequest
            {
                Message = newText
            });
    }

    public Task<ApiResult<DeleteResponse>> DeleteMessage(int chatId, int messageId)
    {
        return DeleteAsync<DeleteResponse>($"api/chat/{chatId}/messages/{messageId}");
    }

    public Task<ApiResult<ImageResponse>> ChangeGroupImage(int chatId, string name, byte[] imageBytes,
        ImageContentType contentType)
    {
        return PatchAsync<ImageResponse>($"api/chat/{chatId}/image", new ImageRequest
        {
            Name = name,
            Bytes = imageBytes,
            ContentType = contentType
        });
    }
}