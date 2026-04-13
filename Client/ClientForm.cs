using Application.DTO;
using Client.ApiClients;
using Client.Properties;
using Client.Realtime;
using Client.Service;
using Client.Utils;
using Contracts.DTO.Chat;
using Domain.Entities;
using Guna.UI2.WinForms;
using System.Threading.Channels;
using static Guna.UI2.Native.WinApi;

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

    private ChatInfo? _currentChat;
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

    private sealed record OutgoingChatMessage(int CurrentChatId, int SenderId, string Text, Guid TempId);

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
            _chatRealtimeClient.MessageUpdated += OnMessageUpdated;
            _chatRealtimeClient.MessageDeleted += OnMessageDeleted;

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

    private void OnMessageReceived(MessageResponse response)
    {
        if (_currentChat == null) return;

        if (InvokeRequired)
        {
            BeginInvoke(() => OnMessageReceived(response));
            return;
        }

        var message = response.Message;
        if (message.ChatId != _currentChat.Id) return;
        if (message.Sender.Id == _currentUser.Id) return;

        Invoke(() => LoadMessage(message));
    }

    private void OnMessageUpdated(MessageResponse response)
    {
        if (_currentChat == null) return;

        if (InvokeRequired)
        {
            BeginInvoke(() => OnMessageUpdated(response));
            return;
        }

        var message = response.Message;
        if (message.ChatId != _currentChat.Id) return;
        if (message.Sender.Id == _currentUser.Id) return;

        var panel = _chatPanel.Controls
            .OfType<Guna2Panel>()
            .FirstOrDefault(p => p.Tag is MessageInfo m && m.Id == message.Id);

        if (panel == null) return;

        var label = panel.Controls.OfType<Label>().FirstOrDefault();
        if (label != null) label.Text = message.Text;

        ((MessageInfo)panel.Tag!).Text = message.Text;
    }

    private void OnMessageDeleted(int messageId)
    {
        if (_currentChat == null) return;

        if (InvokeRequired)
        {
            BeginInvoke(() => OnMessageDeleted(messageId));
            return;
        }

        var panel = _chatPanel.Controls
            .OfType<Guna2Panel>()
            .FirstOrDefault(p => p.Tag is MessageInfo m && m.Id == messageId);

        if (panel == null) return;

        var message = (MessageInfo)panel.Tag!;
        _chatPanel.Controls.Remove(panel);
        _messages?.Remove(message);
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
        addFriendButton.Size = new Size(leftPanel.Width + mainPanel.Width, 45);
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
            _addFriendTxtBox.Size = new Size(leftPanel.Width + mainPanel.Width, 45);
            _addFriendTxtBox.BorderThickness = 0;

            _addFriendTxtBox.KeyDown += AddFriendTxtBox_KeyDown;

            return;
        }

        _isFriendTxtBoxOpen = false;
        _addFriendTxtBox.Visible = false;
    }

    private async void AddFriendTxtBox_KeyDown(object? sender, KeyEventArgs e)
    {
        try
        {
            await HandleAddFriendKeyDown(e);
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

            foreach (var friend in _friendList.Where(friend => !ThereIsAlreadyFriend(friend)))
                CreateFriendPanel(friend);
        }
        finally
        {
            lblLoadingFriends.Visible = false;
        }
    }

    private async Task HandleAddFriendKeyDown(KeyEventArgs e)
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

        if (_currentChat != null)
            await _chatRealtimeClient.LeaveChat(_currentChat.Id);

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
        var searchText = txtBoxSearch.Text;

        if (searchText == "")
        {
            await UpdateFriendList();
            return;
        }

        if (_friendList.Count == 0) return;

        addedFriendPanel.Controls.Clear();

        foreach (var friend in _friendList.Where(friend =>
                     friend.Username != _currentUser.Username &&
                     friend.Username.Contains(searchText, StringComparison.OrdinalIgnoreCase)))
            CreateFriendPanel(friend);
    }

    private void CreateFriendPanel(UserInfo friend)
    {
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
        friendLabel.Tag = friend;
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

        _currentChat = chatResult.Value.Chat;

        foreach (var message in _messages) LoadMessage(message);

        await _chatRealtimeClient.JoinChat(_currentChat.Id);
    }

    private void LoadMessage(MessageInfo message)
    {
        var nameLabel = new Label();
        nameLabel.Text = message.Sender.Username;
        nameLabel.Font = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold);
        nameLabel.ForeColor = Color.LightGray;
        nameLabel.AutoSize = true;
        nameLabel.Tag = message.Sender;

        var messagePanel = new Guna2Panel();
        messagePanel.BackColor = Color.FromArgb(24, 37, 51);
        messagePanel.MaximumSize = new Size(400, 0);
        messagePanel.AutoSize = true;
        messagePanel.Controls.Add(nameLabel);
        messagePanel.Tag = message;
        messagePanel.MouseClick += MessagePanel_MouseClick;

        var messageLabel = new Label();
        messageLabel.Parent = messagePanel;
        messageLabel.Text = message.Text;
        //var options = new JsonSerializerOptions { WriteIndented = true };
        //messageLabel.Text = $"{message.Text}\n{JsonSerializer.Serialize(message, options)}";
        messageLabel.Font = new Font(FontFamily.GenericSansSerif, 12);
        messageLabel.ForeColor = Color.White;
        messageLabel.AutoSize = true;
        messageLabel.MaximumSize = new Size(380, 0);
        messageLabel.Location = new Point(0, 20);

        messagePanel.Controls.Add(messageLabel);

        _chatPanel.Controls.Add(messagePanel);

        _chatPanel.ScrollControlIntoView(messagePanel);
    }

    private async void MessagePanel_MouseClick(object? sender, MouseEventArgs e)
    {
        try
        {
            if ((e.Button & MouseButtons.Right) != 0)
            {
                var panel = (Guna2Panel)sender!;
                var message = (MessageInfo)panel.Tag!;

                ShowMessageContextMenu(panel, message, e.Location);

                //await DeleteMessage(panel, message);

            }
        }
        catch (Exception ex)
        {
            _dialogService.ShowError(ex.Message);
        }
    }

    private void ShowMessageContextMenu(Guna2Panel messagePanel, MessageInfo message, Point location)
    {
        var menuPanel = new Guna2Panel
        {
            Size = new Size(160, 80),
            BackColor = Color.FromArgb(30, 43, 56),
            BorderRadius = 8,
            BorderColor = Color.FromArgb(50, 63, 76),
            BorderThickness = 1
        };

        var editButton = new Guna2Button
        {
            Text = "Изменить",
            Size = new Size(150, 32),
            Location = new Point(5, 5),
            BackColor = Color.Transparent,
            ForeColor = Color.White,
            BorderRadius = 6,
            Font = new Font("Segoe UI", 9f),
            FillColor = Color.Transparent,
            HoverState = { FillColor = Color.FromArgb(45, 58, 71) }
        };

        var deleteButton = new Guna2Button
        {
            Text = "Удалить",
            Size = new Size(150, 32),
            Location = new Point(5, 42),
            BackColor = Color.Transparent,
            ForeColor = Color.FromArgb(220, 80, 80),
            BorderRadius = 6,
            Font = new Font("Segoe UI", 9f),
            FillColor = Color.Transparent,
            HoverState = { FillColor = Color.FromArgb(45, 58, 71) }
        };

        editButton.Click += async (_, _) =>
        {
            menuPanel.Dispose();
            await EditMessage(messagePanel, message);
        };

        deleteButton.Click += async (_, _) =>
        {
            menuPanel.Dispose();
            await DeleteMessage(messagePanel, message);
        };

        menuPanel.Controls.Add(editButton);
        menuPanel.Controls.Add(deleteButton);

        // закрыть при клике вне меню
        menuPanel.LostFocus += (_, _) => menuPanel.Dispose();

        var screenPos = messagePanel.PointToScreen(location);
        var formPos = PointToClient(screenPos);

        menuPanel.Location = formPos;
        Controls.Add(menuPanel);
        menuPanel.BringToFront();
        menuPanel.Focus();
    }

    public async Task EditMessage(Guna2Panel panel, MessageInfo message)
    {
        if (_currentChat == null) return;

        var newText = ShowEditDialog(message.Text);
        if (newText == null) return;

        var result = await _chatApiClient.EditMessage(_currentChat.Id, message.Id, newText);

        if (!result.IsSuccess)
        {
            _dialogService.ShowError(result.ToMessage());
            return;
        }

        var label = panel.Controls.OfType<Label>().FirstOrDefault();
        if (label != null) label.Text = newText;
        message.Text = newText;
    }

    private string? ShowEditDialog(string currentText)
    {
        var form = new Form
        {
            Size = new Size(400, 150),
            StartPosition = FormStartPosition.CenterParent,
            Text = "Изменить сообщение",
            BackColor = Color.FromArgb(23, 33, 43),
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false
        };

        var textBox = new Guna2TextBox
        {
            Text = currentText,
            Location = new Point(10, 10),
            Size = new Size(360, 40),
            ForeColor = Color.White,
            FillColor = Color.FromArgb(30, 43, 56)
        };

        var confirmButton = new Guna2Button
        {
            Text = "Сохранить",
            Location = new Point(270, 60),
            Size = new Size(100, 35),
            FillColor = Color.FromArgb(45, 140, 240),
            ForeColor = Color.White
        };

        string? result = null;
        confirmButton.Click += (_, _) =>
        {
            result = textBox.Text.Trim();
            form.Close();
        };

        form.Controls.Add(textBox);
        form.Controls.Add(confirmButton);
        form.ShowDialog();

        return result;
    }

    public async Task DeleteMessage(Guna2Panel panel, MessageInfo message)
    {
        if (_currentChat == null) return;

        var result = await _chatApiClient.DeleteMessage(_currentChat.Id, message.Id);

        if (!result.IsSuccess)
        {
            _dialogService.ShowError(result.ToMessage());
            return;
        }

        _chatPanel.Controls.Remove(panel);
        _messages?.Remove(message);
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

        var message = new MessageInfo
        {
            Text = text,
            Sender = _currentUser,
            ChatId = _currentChat.Id,
            TempId = Guid.NewGuid()
        };

        LoadMessage(message);

        await _messageQueue.Writer.WriteAsync(new OutgoingChatMessage(
            _currentChat.Id,
            _currentUser.Id,
            text,
            message.TempId));
    }

    private async Task ProcessOutgoingMessages(CancellationToken token)
    {
        try
        {
            await foreach (var item in _messageQueue.Reader.ReadAllAsync(token))
            {
                var result = await _chatApiClient.SendMessage(item.CurrentChatId, item.SenderId, item.Text);

                if (result is { IsSuccess: true, Value: not null })
                {
                    UpdateMessage(item, result.Value.Message);
                    continue;
                }

                if (!IsDisposed)
                    BeginInvoke(() => _dialogService.ShowError(result.ToMessage()));
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    private void UpdateMessage(OutgoingChatMessage item, MessageInfo message)
    {
        BeginInvoke(() =>
        {
            var panel = _chatPanel.Controls
                .OfType<Guna2Panel>()
                .FirstOrDefault(p => p.Tag is MessageInfo m && m.TempId == item.TempId);

            if (panel?.Tag is not MessageInfo) return;

            message.TempId = item.TempId;
            panel.Tag = message;

            /*var label = panel.Controls.OfType<Label>()
                .FirstOrDefault(l => l.Location.Y == 20);

            if (label != null)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                label.Text = $"{message.Text}\n{JsonSerializer.Serialize(message, options)}";
            }*/
        });
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