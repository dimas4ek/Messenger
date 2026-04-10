using System.Threading.Channels;
using Application.DTO;
using Client.ApiClients;
using Client.Properties;
using Client.Realtime;
using Client.Service;
using Client.Utils;
using Contracts.DTO.Chat;
using Guna.UI2.WinForms;

namespace Client;

public partial class ClientForm : Form
{
    private readonly AuthApiClient _authApiClient;
    private readonly ChatApiClient _chatApiClient;
    private readonly ChatRealtimeClient _chatRealtimeClient;

    private readonly IDialogService _dialogService;
    private readonly FriendApiClient _friendApiClient;

    private readonly Channel<OutgoingChatMessage> _messageQueue =
        Channel.CreateBounded<OutgoingChatMessage>(new BoundedChannelOptions(100)
        {
            SingleReader = true,
            SingleWriter = false,
            FullMode = BoundedChannelFullMode.Wait
        });

    private readonly UserApiClient _userApiClient;

    private readonly UserContext _userContext;
    private Guna2TextBox _addFriendTxtBox;

    private FlowLayoutPanel _chatPanel;

    private UserInfo? _companion;
    private UserInfo _currentUser;

    private List<UserInfo> _friendList = [];
    private bool _isFriendTxtBoxOpen;
    private bool _isProfileOpen;
    private List<MessageInfo>? _messages = [];

    private Guna2Panel _profilePanel;

    private CancellationTokenSource? _sendMessagesCts;

    public ClientForm(
        UserContext userContext, IDialogService dialogService, FriendApiClient friendApiClient,
        UserApiClient userApiClient, ChatApiClient chatApiClient, ChatRealtimeClient chatRealtimeClient,
        AuthApiClient authApiClient)
    {
        _userContext = userContext;
        _dialogService = dialogService;
        _friendApiClient = friendApiClient;
        _userApiClient = userApiClient;
        _chatApiClient = chatApiClient;
        _chatRealtimeClient = chatRealtimeClient;
        _authApiClient = authApiClient;

        InitializeComponent();

        Load += ClientForm_Load;
    }

    public void btnUpdServers_Click(object sender, EventArgs e)
    {
        //todo
    }

    private sealed record OutgoingChatMessage(int SenderId, int CompanionId, string Text);

    #region Main Things

    public async void ClientForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _currentUser = _userContext.CurrentUser ?? throw new Exception("Пользователь не авторизован");

            DoImportantThings();
            CreateProfilePanel();
            await UpdateFriendList();

            await _chatRealtimeClient.Connect(_currentUser.Id);
            _chatRealtimeClient.MessageReceived += OnMessageReceived;

            _sendMessagesCts = new CancellationTokenSource();
            _ = ProcessOutgoingMessages(_sendMessagesCts.Token);
        }
        catch (Exception ex)
        {
            _dialogService.ShowError(ex.Message);
        }
    }

    private async void ClientForm_Close(object sender, FormClosingEventArgs e)
    {
        try
        {
            await _chatRealtimeClient.Disconnect();
            if (_sendMessagesCts != null) await _sendMessagesCts.CancelAsync();

            _messageQueue.Writer.TryComplete();

            var currentUser = _userContext.CurrentUser;
            if (currentUser != null)
            {
                var result = await _authApiClient.Logout(currentUser.Id);
                if (!result.IsSuccess || result.Value == null) _dialogService.ShowError(result.ToMessage());
                _userContext.Clear();
            }

            Environment.Exit(0);
        }
        catch (Exception ex)
        {
            _dialogService.ShowError(ex.Message);
        }
    }

    public void OnMessageReceived(MessageResponse response)
    {
        var message = response.Message;
        var messageSender = message.Sender;
        if (_companion == null) return;
        if (messageSender.Id == _companion.Id || messageSender.Id == _currentUser.Id)
            Invoke(() => LoadMessage(message.Text, messageSender.Username));
    }

    private void DoImportantThings()
    {
        availableServers.Visible = false;
        companionPanel.Visible = false;
        txtBoxMessage.Visible = false;
        txtBoxPanel.Visible = false;
        btnSndMsg.Visible = false;
        btnUpdServers.Visible = false;
    }

    private void CreateProfilePanel()
    {
        _profilePanel = new Guna2Panel();
        _profilePanel.Size = new Size(leftPanel.Width + mainPanel.Width, Height);
        _profilePanel.BackColor = Color.FromArgb(23, 33, 43);
        _profilePanel.Location = new Point(0, 0);
        _profilePanel.Dock = DockStyle.Left;

        Controls.Add(_profilePanel);

        var profileUsernameLabel = new Label();
        profileUsernameLabel.Parent = _profilePanel;
        profileUsernameLabel.Location = new Point(75, 20);
        profileUsernameLabel.Font = new Font(FontFamily.GenericSansSerif, 15.75f);
        profileUsernameLabel.ForeColor = Color.White;
        profileUsernameLabel.Text = _currentUser.Username;

        var btnCloseProfile = new Guna2ImageButton();
        btnCloseProfile.Parent = _profilePanel;
        btnCloseProfile.Location = new Point(5, 6);
        btnCloseProfile.Size = new Size(64, 52);

        btnCloseProfile.Image = Resources.btnProfileImage;
        var imageSize = new Size(40, 40);
        btnCloseProfile.ImageSize = imageSize;
        btnCloseProfile.HoverState.ImageSize = imageSize;
        btnCloseProfile.CheckedState.ImageSize = imageSize;
        btnCloseProfile.PressedState.ImageSize = imageSize;

        btnCloseProfile.Click += btnCloseProfile_Click;

        var addFriendButton = new Guna2Button();
        addFriendButton.Parent = _profilePanel;
        addFriendButton.Location = new Point(0, 58);
        addFriendButton.Size = new Size(285, 45);
        addFriendButton.FillColor = Color.FromArgb(23, 33, 43);
        addFriendButton.HoverState.FillColor = Color.FromArgb(35, 46, 60);
        addFriendButton.Text = @"Add Friend";
        addFriendButton.Font = new Font("Segoe UI", 12);
        addFriendButton.TextAlign = HorizontalAlignment.Center;

        addFriendButton.Click += AddFriendButton_Click;
        _addFriendTxtBox = new Guna2TextBox();

        _profilePanel.Visible = false;
    }

    #endregion

    #region Friends

    private void AddFriendButton_Click(object? sender, EventArgs e)
    {
        if (!_isFriendTxtBoxOpen)
        {
            _isFriendTxtBoxOpen = true;

            _addFriendTxtBox.Visible = true;
            _addFriendTxtBox.Parent = _profilePanel;
            _addFriendTxtBox.PlaceholderText = "Enter Friend name...";
            _addFriendTxtBox.PlaceholderForeColor = Color.FromArgb(193, 200, 207);
            _addFriendTxtBox.FillColor = Color.FromArgb(35, 46, 60);
            _addFriendTxtBox.Location = new Point(0, 103);
            _addFriendTxtBox.Size = new Size(285, 45);
            _addFriendTxtBox.BorderThickness = 0;

            _addFriendTxtBox.KeyUp += AddFriendTxtBox_KeyUp;

            return;
        }

        _isFriendTxtBoxOpen = false;
        _addFriendTxtBox.Visible = false;
    }

    private async void AddFriendTxtBox_KeyUp(object? sender, KeyEventArgs e)
    {
        try
        {
            await HandleAddFriendKeyUp(e);
        }
        catch (Exception ex)
        {
            _dialogService.ShowError(ex.Message);
        }
    }

    private static void FriendLabel_MouseLeave(object? sender, EventArgs e)
    {
        if (sender is not Label label) return;

        SetFriendLabelColor(label);
    }

    private static void FriendLabel_MouseMove(object? sender, MouseEventArgs e)
    {
        if (sender is not Label label) return;

        label.Cursor = Cursors.Hand;

        SetFriendLabelColor(label);
    }

    private static void FriendPanel_MouseLeave(object? sender, EventArgs e)
    {
        if (sender is not Guna2Panel panel) return;

        SetFriendPanelColor(panel, 23, 33, 43);
    }

    private static void FriendPanel_MouseMove(object? sender, MouseEventArgs e)
    {
        if (sender is not Guna2Panel panel) return;

        panel.Cursor = Cursors.Hand;

        SetFriendPanelColor(panel, 35, 46, 60);
    }

    private async void FriendPanel_MouseClick(object? sender, MouseEventArgs e)
    {
        try
        {
            await HandleFriendMouseClick(sender);
        }
        catch (Exception ex)
        {
            _dialogService.ShowError(ex.Message);
        }
    }

    private async Task UpdateFriendList()
    {
        addedFriendPanel.Controls.Clear();
        lblLoadingFriends.Visible = true;

        try
        {
            var friendListResult = await _friendApiClient.GetFriendList(_currentUser.Id);
            if (!friendListResult.IsSuccess || friendListResult.Value == null)
            {
                _dialogService.ShowError(friendListResult.ToMessage());
                return;
            }

            _friendList = friendListResult.Value.Friends;

            foreach (var friend in _friendList)
            {
                if (ThereIsAlreadyFriend(friend)) continue;

                var friendPanel = new Guna2Panel();
                friendPanel.Parent = addedFriendPanel;
                friendPanel.Size = new Size(210, 70);
                friendPanel.BackColor = Color.FromArgb(23, 33, 43);
                friendPanel.MouseMove += FriendPanel_MouseMove;
                friendPanel.MouseLeave += FriendPanel_MouseLeave;
                friendPanel.MouseClick += FriendPanel_MouseClick;
                friendPanel.Dock = DockStyle.Top;
                friendPanel.Tag = friend;

                var friendLabel = new Label();
                friendLabel.Parent = friendPanel;
                friendLabel.Text = friend.Username;
                friendLabel.Location = new Point(6, 13);
                friendLabel.Font = new Font(FontFamily.GenericSansSerif, 12);
                friendLabel.BackColor = Color.FromArgb(23, 33, 43);
                friendLabel.ForeColor = Color.White;
                friendLabel.MouseMove += FriendLabel_MouseMove;
                friendLabel.MouseLeave += FriendLabel_MouseLeave;
                friendLabel.MouseClick += FriendPanel_MouseClick;
                friendLabel.Tag = friend;
            }
        }
        finally
        {
            lblLoadingFriends.Visible = false;
        }
    }

    private async Task HandleAddFriendKeyUp(KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        var friendName = _addFriendTxtBox.Text;
        if (string.IsNullOrWhiteSpace(friendName)) return;

        if (friendName == _currentUser.Username)
        {
            _dialogService.ShowMessage("You can't add yourself as a Friend");
            return;
        }

        var friendResult = await _userApiClient.Get(friendName);
        var friend = friendResult.Value;

        if (!friendResult.IsSuccess || friend == null)
        {
            _dialogService.ShowError(friendResult.ToMessage());
            return;
        }

        var friendUser = friend.User;

        var alreadyFriends = await _friendApiClient.AlreadyFriends(_currentUser.Id, friendUser.Id);
        if (!alreadyFriends.IsSuccess || alreadyFriends.Value == null)
        {
            _dialogService.ShowError(alreadyFriends.ToMessage());
            return;
        }

        if (alreadyFriends.Value.IsFriends)
        {
            _dialogService.ShowMessage("You are already friends with this user!");
            return;
        }

        var result = await _friendApiClient.Add(_currentUser.Id, friendUser.Id);
        if (!result.IsSuccess)
        {
            _dialogService.ShowError(result.ToMessage());
            return;
        }

        _dialogService.ShowMessage("Participant has been added to your Friend list!");

        await UpdateFriendList();

        _addFriendTxtBox.Clear();
    }

    private static void SetFriendLabelColor(Label label)
    {
        label.BackColor = Color.FromArgb(35, 46, 60);

        if (label.Parent is Guna2Panel panel) panel.BackColor = Color.FromArgb(35, 46, 60);
    }

    private static void SetFriendPanelColor(Guna2Panel panel, int r, int g, int b)
    {
        panel.BackColor = Color.FromArgb(r, g, b);

        var label = panel.Controls.OfType<Label>().FirstOrDefault();
        if (label == null) return;

        label.BackColor = Color.FromArgb(r, g, b);
        label.ForeColor = Color.White;
    }

    private async Task HandleFriendMouseClick(object? sender)
    {
        var user = sender switch
        {
            Guna2Panel panel => panel.Tag as UserInfo,
            Label label => label.Tag as UserInfo,
            _ => null
        };

        if (user == null) return;

        if (_companion != null && _companion.Id == user.Id)
            return;

        _companion = user;
        lblCompanionUsername.Text = _companion.Username;

        chatPanelGuna.Controls.Clear();

        await LoadDialog();
    }

    private bool ThereIsAlreadyFriend(UserInfo friend)
    {
        return (
                from Guna2Panel panel in addedFriendPanel.Controls
                select panel.Controls.OfType<Label>().FirstOrDefault())
            .Any(label => label?.Text == friend.Username);
    }

    private async void txtBoxSearch_KeyUp(object sender, KeyEventArgs e)
    {
        try
        {
            await HandleSearchKeyUp();
        }
        catch (Exception ex)
        {
            _dialogService.ShowError(ex.Message);
        }
    }

    private async Task HandleSearchKeyUp()
    {
        if (txtBoxSearch.Text == "")
        {
            await UpdateFriendList();
            return;
        }

        if (_friendList.Count == 0) return;

        addedFriendPanel.Controls.Clear();

        foreach (var friend in _friendList)
        {
            if (friend.Username == _currentUser.Username) continue;

            var friendPanel = new Guna2Panel();
            friendPanel.Parent = addedFriendPanel;
            friendPanel.Size = new Size(210, 70);
            friendPanel.BackColor = Color.FromArgb(23, 33, 43);
            friendPanel.MouseMove += FriendPanel_MouseMove;
            friendPanel.MouseLeave += FriendPanel_MouseLeave;
            friendPanel.MouseClick += FriendPanel_MouseClick;
            friendPanel.Dock = DockStyle.Top;

            var friendLabel = new Label();
            friendLabel.Parent = friendPanel;
            friendLabel.Text = friend.Username;
            friendLabel.Location = new Point(6, 13);
            friendLabel.Font = new Font(FontFamily.GenericSansSerif, 12);
            friendLabel.BackColor = Color.FromArgb(23, 33, 43);
            friendLabel.ForeColor = Color.White;
            friendLabel.MouseMove += FriendLabel_MouseMove;
            friendLabel.MouseLeave += FriendLabel_MouseLeave;
        }
    }

    #endregion

    #region Chats

    private async Task LoadDialog()
    {
        _chatPanel = new FlowLayoutPanel();
        _chatPanel.Parent = chatPanelGuna;

        _chatPanel.Dock = DockStyle.Fill;
        _chatPanel.FlowDirection = FlowDirection.TopDown;
        _chatPanel.WrapContents = false;
        _chatPanel.AutoScroll = true;

        companionPanel.Visible = true;
        btnSndMsg.Visible = true;
        txtBoxMessage.Visible = true;
        txtBoxPanel.Visible = true;

        if (_companion == null) return;
        var chatResult = await _chatApiClient.LoadPrivateChat(_currentUser.Id, _companion.Id);
        if (!chatResult.IsSuccess || chatResult.Value == null)
        {
            _dialogService.ShowError("Failed to load chat: " + chatResult.ToMessage());
            return;
        }

        _messages = chatResult.Value.Chat.Messages;
        if (_messages == null)
        {
            _dialogService.ShowError("Failed to load messages: Messages are null");
            return;
        }

        foreach (var message in _messages) LoadMessage(message.Text, message.Sender.Username);
    }

    private void LoadMessage(string message, string userName)
    {
        var nameLabel = new Label();
        nameLabel.Text = userName;
        nameLabel.Font = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold);
        nameLabel.ForeColor = Color.LightGray;
        nameLabel.AutoSize = true;

        var messagePanel = new Guna2Panel();
        messagePanel.BackColor = Color.FromArgb(24, 37, 51);
        messagePanel.MaximumSize = new Size(400, 0);
        messagePanel.AutoSize = true;
        messagePanel.Controls.Add(nameLabel);

        var messageLabel = new Label();
        messageLabel.Parent = messagePanel;
        messageLabel.Text = message;
        messageLabel.Font = new Font(FontFamily.GenericSansSerif, 12);
        messageLabel.ForeColor = Color.White;
        messageLabel.AutoSize = true;
        messageLabel.MaximumSize = new Size(380, 0);
        messageLabel.Location = new Point(0, 20);

        messagePanel.Controls.Add(messageLabel);

        _chatPanel.Controls.Add(messagePanel);

        _chatPanel.ScrollControlIntoView(messagePanel);
    }

    private async void btnSndMsg_Click(object sender, EventArgs e)
    {
        try
        {
            await EnqueueMessage();
        }
        catch (Exception ex)
        {
            _dialogService.ShowError(ex.Message);
        }
    }

    private async void txtBoxMessage_KeyDown(object sender, KeyEventArgs e)
    {
        try
        {
            if (e.KeyCode != Keys.Enter) return;

            e.SuppressKeyPress = true;
            e.Handled = true;

            await EnqueueMessage();
        }
        catch (Exception ex)
        {
            _dialogService.ShowError(ex.Message);
        }
    }

    private async Task EnqueueMessage()
    {
        var text = txtBoxMessage.Text.Trim();
        if (string.IsNullOrWhiteSpace(text)) return;
        if (_companion == null) return;

        txtBoxMessage.Clear();

        LoadMessage(text, _currentUser.Username);

        await _messageQueue.Writer.WriteAsync(new OutgoingChatMessage(
            _currentUser.Id,
            _companion.Id,
            text));
    }

    private async Task ProcessOutgoingMessages(CancellationToken token)
    {
        try
        {
            await foreach (var item in _messageQueue.Reader.ReadAllAsync(token))
            {
                var result = await _chatApiClient.SendMessage(item.SenderId, item.CompanionId, item.Text);

                if (result is { IsSuccess: true, Value: not null }) continue;

                if (!IsDisposed)
                    BeginInvoke(() => _dialogService.ShowError(result.ToMessage()));
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    #endregion

    #region Profile

    private void btnOpenProfile_Click(object sender, EventArgs e)
    {
        OpenProfile();
    }

    private void btnCloseProfile_Click(object? sender, EventArgs e)
    {
        CloseProfile();
    }

    private void OpenProfile()
    {
        if (_isProfileOpen) return;

        _isProfileOpen = true;

        _profilePanel.Visible = true;

        leftPanel.Visible = false;
        addedFriendPanel.Visible = false;
        mainPanel.Visible = false;
        searchPanel.Visible = false;
        txtBoxSearch.Visible = false;
    }

    private void CloseProfile()
    {
        if (!_isProfileOpen) return;

        _isProfileOpen = false;

        _profilePanel.Visible = false;

        leftPanel.Visible = true;
        addedFriendPanel.Visible = true;
        mainPanel.Visible = true;
        searchPanel.Visible = true;
        txtBoxSearch.Visible = true;
    }

    #endregion
}