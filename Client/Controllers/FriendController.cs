using Application.DTO;
using Client.Api.Clients;
using Client.Services;
using Client.Utils;

namespace Client.Controllers;

public class FriendController(
    FriendApiClient friendApiClient,
    UserApiClient userApiClient,
    IDialogService dialogService)
{
    private readonly IDialogService _dialogService = dialogService;
    private readonly FriendApiClient _friendApiClient = friendApiClient;
    private readonly UserApiClient _userApiClient = userApiClient;

    public List<UserInfo> Friends { get; private set; } = [];

    public event Action<UserInfo>? FriendAdded;

    public async Task<List<UserInfo>> LoadFriendsAsync(int userId)
    {
        var result = await _friendApiClient.GetFriendList(userId);
        if (!result.IsSuccess || result.Value == null)
        {
            _dialogService.ShowError(result.ToMessage());
            return [];
        }

        Friends = result.Value.Friends;
        return Friends;
    }

    public async Task AddFriendAsync(int currentUserId, string currentUsername, string friendName)
    {
        if (string.IsNullOrWhiteSpace(friendName)) return;

        if (friendName == currentUsername)
        {
            _dialogService.ShowMessage("You can't add yourself as a Friend");
            return;
        }

        var userResult = await _userApiClient.Get(friendName);
        if (!userResult.IsSuccess || userResult.Value == null)
        {
            _dialogService.ShowError(userResult.ToMessage());
            return;
        }

        var friendUser = userResult.Value.User;

        var alreadyResult = await _friendApiClient.AlreadyFriends(currentUserId, friendUser.Id);
        if (!alreadyResult.IsSuccess || alreadyResult.Value == null)
        {
            _dialogService.ShowError(alreadyResult.ToMessage());
            return;
        }

        if (alreadyResult.Value.IsFriends)
        {
            _dialogService.ShowMessage("You are already friends with this user!");
            return;
        }

        var addResult = await _friendApiClient.Add(currentUserId, friendUser.Id);
        if (!addResult.IsSuccess)
        {
            _dialogService.ShowError(addResult.ToMessage());
            return;
        }

        Friends.Add(friendUser);
        FriendAdded?.Invoke(friendUser);
        _dialogService.ShowMessage($"{friendName} has been added to your Friend list!");
    }

    public List<UserInfo> Search(string query, string currentUsername)
    {
        if (string.IsNullOrWhiteSpace(query)) return Friends;

        return Friends
            .Where(f =>
                f.Username != currentUsername &&
                f.Username.Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}