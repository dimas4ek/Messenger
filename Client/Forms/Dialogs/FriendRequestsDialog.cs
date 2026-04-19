using System.Diagnostics;
using Application.DTO;
using Client.Api.Clients;
using Client.Services;
using Client.UI;
using Client.Utils;
using Guna.UI2.WinForms;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Forms.Dialogs;

public class FriendRequestsDialog : Form
{
    private readonly UserInfo _currentUser;
    private readonly IDialogService _dialogService;
    private readonly FriendApiClient _friendApiClient;

    private readonly Action<UserInfo> _onFriendAdded;

    public FriendRequestsDialog(UserInfo currentUser, Action<UserInfo> onFriendAdded)
    {
        _currentUser = currentUser;
        _onFriendAdded = onFriendAdded;

        _dialogService = App.Services.GetRequiredService<IDialogService>();
        _friendApiClient = App.Services.GetRequiredService<FriendApiClient>();

        Debug.WriteLine("FriendRequestsDialog");
        InitializeComponent();
    }

    private async void InitializeComponent()
    {
        Size = new Size(400, 420);
        StartPosition = FormStartPosition.CenterParent;
        Text = "Friend Requests";
        BackColor = Color.FromArgb(23, 33, 43);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        var result = await _friendApiClient.GetFriendRequests(_currentUser.Id);
        if (!result.IsSuccess || result.Value == null) return;

        foreach (var request in result.Value.FriendRequests)
        {
            Debug.WriteLine("Friend request: " + request.Id);
            AddFriendRequestPanel(request);
        }
    }

    private void AddFriendRequestPanel(FriendRequestInfo request)
    {
        var panel = FriendRequestPanelFactory.Create(
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
            (s, e) => HandleFriendRequestAction(s, true),
            (s, e) => HandleFriendRequestAction(s, false)
            /*(s, e) => _ = SafeInvoke(() => HandleFriendRequestAction(s, true)),
            (s, e) => _ = SafeInvoke(() => HandleFriendRequestAction(s, false))*/
        );

        Controls.Add(panel);
    }

    private async Task HandleFriendRequestAction(object? s, bool accept)
    {
        if (s is not Control { Tag: FriendRequestInfo request }) return;

        var result = await _friendApiClient.FriendRequestAction(request.Id, accept);

        if (!result.IsSuccess || result.Value == null)
        {
            _dialogService.ShowError(result.ToMessage());
            return;
        }

        if (accept && result.Value?.AddedFriend != null)
            _onFriendAdded(result.Value.AddedFriend);

        RemoveRequestPanel(request);
    }

    private void RemoveRequestPanel(FriendRequestInfo request)
    {
        if (Controls
                .Find("requestPanel", false)
                .FirstOrDefault(p => p.Tag is FriendRequestInfo r && r.Id == request.Id) is
            Guna2Panel panel)
            Controls.Remove(panel);
    }
}