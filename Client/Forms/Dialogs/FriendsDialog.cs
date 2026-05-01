using Application.DTO;
using Client.Api.Clients;
using Client.Forms.Base;
using Client.Services;
using Client.UI.UserControls;
using Client.UI.Utils;
using Client.Utils;
using Guna.UI2.WinForms;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Forms.Dialogs;

public partial class FriendsDialog : BaseForm
{
    private readonly IDialogService _dialogService;
    private readonly FriendApiClient _friendApiClient;

    private readonly List<UserInfo> _friends;

    private readonly Action<ChatInfo> _onFriendAdded;

    public FriendsDialog(UserInfo currentUser, List<UserInfo> friends, Action<ChatInfo> onFriendAdded)
    {
        _friends = friends;
        _onFriendAdded = onFriendAdded;

        _dialogService = App.Services.GetRequiredService<IDialogService>();
        _friendApiClient = App.Services.GetRequiredService<FriendApiClient>();

        InitializeComponent();

        friendListLabel.Location = new Point(
            (friendListTopPanel.Width - friendListLabel.PreferredWidth) / 2,
            (friendListTopPanel.Height - friendListLabel.PreferredHeight) / 2);

        friendRequestsLabel.Location = new Point(
            (friendRequestTopPanel.Width - friendRequestsLabel.PreferredWidth) / 2,
            (friendRequestTopPanel.Height - friendRequestsLabel.PreferredHeight) / 2);

        noneFriendsLabel.Location = new Point(
            (friendListPanel.Width - noneFriendsLabel.PreferredWidth) / 2, 20);

        noneFriendsRequestsLabel.Location = new Point(
            (friendRequestsPanel.Width - noneFriendsRequestsLabel.PreferredWidth) / 2, 20);

        Load += async (_, _) =>
        {
            noneFriendsLabel.Visible = _friends.Count == 0;

            foreach (var friend in _friends)
                AddFriendPanel(friend);

            var friendRequestsResult = await _friendApiClient.GetFriendRequests(currentUser.Id);
            var friendRequestsValue = friendRequestsResult.Value;

            if (!friendRequestsResult.IsSuccess || friendRequestsValue == null) return;

            var friendRequests = friendRequestsValue.FriendRequests;

            noneFriendsRequestsLabel.Visible = friendRequests.Count == 0;

            foreach (var request in friendRequests)
                AddFriendRequestPanel(request);
        };
    }

    private void AddFriendPanel(UserInfo friend)
    {
        var panel = new FriendPanelUC(
            friend,
            (s, _) => MouseEventUtils.OnFriendPanelMove(s, p => ColorHelper.SetPanelColor(p, 35, 46, 60)),
            (s, _) => MouseEventUtils.OnFriendPanelMove(s, p => ColorHelper.SetPanelColor(p, 23, 33, 43))
        );

        friendListPanel.Controls.Add(panel);
    }

    private void AddFriendRequestPanel(FriendRequestInfo request)
    {
        var panel = new FriendRequestPanelUC(
            request,
            (s, _) =>
            {
                if (s is Control c)
                    ColorHelper.SetPanelColor((Guna2Panel)(c is Label l ? l.Parent! : c), 35, 46, 60);
            },
            (s, _) =>
            {
                if (s is Control c)
                    ColorHelper.SetPanelColor((Guna2Panel)(c is Label l ? l.Parent! : c), 23, 33, 43);
            },
            (s, e) => _ = SafeInvoke(() => HandleFriendRequestAction(s, true)),
            (s, e) => _ = SafeInvoke(() => HandleFriendRequestAction(s, false))
        );

        friendRequestsPanel.Controls.Add(panel);
    }

    private async Task HandleFriendRequestAction(object? s, bool accept)
    {
        if (s is not Control { Tag: FriendRequestInfo request }) return;

        var result = await _friendApiClient.FriendRequestAction(request.Id, accept);

        var friendRequest = result.Value;

        if (!result.IsSuccess || friendRequest == null)
        {
            _dialogService.ShowError(result.ToMessage());
            return;
        }

        if (accept && friendRequest is { CreatedChat: not null, Friend: not null })
        {
            _onFriendAdded(friendRequest.CreatedChat);
            _friends.Add(friendRequest.Friend);
        }

        RemoveRequestPanel(request);
    }

    private void RemoveRequestPanel(FriendRequestInfo request)
    {
        var panel = friendRequestsPanel.Controls.OfType<FriendRequestPanelUC>()
            .FirstOrDefault(p => p.Tag is FriendRequestInfo r && r.Id == request.Id);
        friendRequestsPanel.Controls.Remove(panel);
    }
}