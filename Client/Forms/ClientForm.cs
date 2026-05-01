using Application.DTO;
using Client.Api.Clients;
using Client.Controllers;
using Client.Filter;
using Client.Forms.Base;
using Client.Forms.Dialogs;
using Client.Services;
using Client.UI.UserControls;
using Client.UI.Utils;
using Contracts.DTO.Event;
using Domain.Enums;
using Guna.UI2.WinForms;
using static Client.Services.MessageQueueService;

namespace Client.Forms;

public partial class ClientForm : BaseForm
{
    private readonly ChatApiClient _chatApiClient = null!;
    private readonly ChatController _chatController = null!;
    private readonly FriendController _friendController = null!;
    private readonly ProfileController _profileController = null!;
    private readonly UserContext _userContext = null!;

    private Guna2Panel? _activeMenuPanel;
    private ChatInfo? _chat;

    private UserInfo _currentUser = null!;
    private OutsideClickFilter? _menuFilter;

    private MessageQueueService? _messageQueue;

    private FlowLayoutPanel _messagesPanel = null!;

    private ProfilePanelUC _profilePanel = null!;

    public ClientForm()
    {
    }

    public ClientForm(ChatController chatController,
        FriendController friendController,
        ProfileController profileController,
        UserContext userContext,
        IDialogService dialogService,
        ChatApiClient chatApiClient) : base(dialogService)
    {
        _chatController = chatController;
        _friendController = friendController;
        _profileController = profileController;
        _userContext = userContext;
        _chatApiClient = chatApiClient;

        InitializeComponent();

        Load += ClientForm_Load;

        _chatController.GroupChatCreated += OnGroupChatCreated;
        _chatController.ChatDeleted += OnChatDeleted;

        _chatController.MessageLoaded += OnMessageLoaded;
        _chatController.MessageReceived += OnMessageReceived;
        _chatController.MessageUpdated += OnMessageUpdated;
        _chatController.MessageDeleted += OnMessageDeleted;

        _friendController.FriendAdded += OnFriendAdded;
        _friendController.FriendUpdated += OnFriendUpdated;
        _friendController.FriendStatus += OnFriendStatus;
    }

    #region Load / Close

    private void ClientForm_Load(object? sender, EventArgs e)
    {
        _currentUser = _userContext.CurrentUser ?? throw new Exception("Пользователь не авторизован");

        DoImportantThings();
        SetupButtons(btnOpenProfile, btnFriendRequests, btnCreateGroupChat);
        CreateProfilePanel();

        _ = SafeInvoke(async () =>
        {
            await _chatController.ConnectAsync(_currentUser.Id);
            await _friendController.ConnectAsync(_currentUser.Id);

            await _friendController.LoadFriends(_currentUser.Id);
            await _chatController.LoadChats(_currentUser.Id);

            foreach (var chat in _chatController.Chats)
                AddChatPanel(chat);
            lblLoadingChats.Visible = false;

            _messageQueue = new MessageQueueService(_chatApiClient, DialogService);
            _messageQueue.MessageConfirmed += OnMessageConfirmed;
            _messageQueue.Start();
        });

        DoubleBuffered = true;
    }

    private void ClientForm_Close(object sender, FormClosingEventArgs e)
    {
        _ = SafeInvoke(async () =>
        {
            await _chatController.DisconnectAsync();
            await _friendController.DisconnectAsync();

            if (_messageQueue != null)
                await _messageQueue.DisposeAsync();

            await _profileController.LogoutAsync();

            Environment.Exit(0);
        });
    }

    private void DoImportantThings()
    {
        availableServers.Visible = false;
        chatTopPanel.Visible = false;
        txtBoxMessage.Visible = false;
        txtBoxPanel.Visible = false;
        btnSndMsg.Visible = false;
        btnUpdServers.Visible = false;
    }

    private void SetupButtons(params Guna2ImageButton[] buttons)
    {
        foreach (var button in buttons)
        {
            button.HoverState.ImageSize = button.ImageSize;
            button.PressedState.ImageSize = button.ImageSize;
            button.MouseMove += (_, _) => Cursor = Cursors.Hand;
            button.MouseLeave += (_, _) => Cursor = Cursors.Default;
        }
    }

    #endregion

    #region Chat Events

    private void OnGroupChatCreated(GroupChatCreatedEvent e)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => OnGroupChatCreated(e));
            return;
        }

        AddChatPanel(e.Chat);
    }

    private void OnChatDeleted(int chatId)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => OnChatDeleted(chatId));
            return;
        }

        var panel = FindPrivateChatPanel(chatId);
        if (panel == null) return;

        chatListPanel.Controls.Remove(panel);
    }

    private void OnMessageLoaded(MessageInfo message)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => OnMessageLoaded(message));
            return;
        }

        LoadMessageUI(message);
    }

    private void OnMessageReceived(MessageInfo message)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => OnMessageReceived(message));
            return;
        }

        if (message.Sender.Id == _currentUser.Id) return;
        LoadMessageUI(message);
    }

    private void OnMessageUpdated(MessageInfo message)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => OnMessageUpdated(message));
            return;
        }

        var panel = FindMessagePanel(message.Id);
        if (panel == null) return;

        panel.UpdateText(message.Text);
        if (panel.Tag is MessageInfo m)
            m.Text = message.Text;
    }

    private void OnMessageDeleted(int messageId)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => OnMessageDeleted(messageId));
            return;
        }

        var panel = FindMessagePanel(messageId);
        if (panel == null) return;

        _messagesPanel.Controls.Remove(panel);
    }

    private void OnMessageConfirmed(OutgoingChatMessage item, MessageInfo message)
    {
        BeginInvoke(() =>
        {
            var panel = _messagesPanel.Controls
                .OfType<MessagePanelUC>()
                .FirstOrDefault(p => p.Message.TempId == item.TempId);

            if (panel == null) return;

            message.TempId = item.TempId;
            panel.ConfirmMessage(message);
        });
    }

    #endregion

    #region Friend Events

    private void OnFriendAdded(FriendAddedEvent e)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => OnFriendAdded(e));
            return;
        }

        _friendController.Friends.Add(e.Friend);
        AddChatPanel(e.FriendChat);
    }

    private void OnFriendUpdated(FriendUpdatedEvent e)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => OnFriendUpdated(e));
            return;
        }

        var user = e.User;

        var friend = _friendController.Friends.FirstOrDefault(f => f.Id == user.Id);
        if (friend == null) return;

        if (e.UsernameChanged) friend.Username = user.Username;
        if (e.AvatarChanged) friend.Avatar = user.Avatar;

        var panel = FindPrivateChatPanel(user.Id);
        if (panel == null) return;

        if (e.UsernameChanged) panel.UpdateText(friend.Username);
        if (e.AvatarChanged) panel.UpdateAvatar(friend.Avatar!);

        if (panel.Tag is not ChatInfo chat) return;

        if (e.UsernameChanged) chat.Name = friend.Username;
        if (e.AvatarChanged) chat.Image = friend.Avatar;
    }

    private void OnFriendStatus(FriendStatusEvent e)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => OnFriendStatus(e));
            return;
        }

        var panel = FindPrivateChatPanel(e.User.Id);
        if (panel == null) return;

        if (e.StatusChanged)
            panel.UpdateStatus(e.User.Status);
    }

    #endregion

    #region Friends

    private void AddFriendTxtBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        _ = SafeInvoke(async () =>
        {
            await _friendController.SendFriendRequest(_currentUser.Id, _currentUser.Username,
                _profilePanel.FriendTextBoxText);
            _profilePanel.ClearFriendTextBox();
        });
    }

    private void txtBoxSearch_KeyUp(object sender, KeyEventArgs e)
    {
        _ = SafeInvoke(() =>
        {
            var chats = string.IsNullOrWhiteSpace(txtBoxSearch.Text)
                ? _chatController.Chats
                : _chatController.Search(txtBoxSearch.Text);

            chatListPanel.Controls.Clear();
            foreach (var chat in chats)
                AddChatPanel(chat);
            return Task.CompletedTask;
        });
    }

    private void BtnFriendRequestsClick(object sender, EventArgs e)
    {
        var dialog = new FriendsDialog(_currentUser, _friendController.Friends, AddChatPanel);
        dialog.ShowDialog();
    }

    #endregion

    #region Chat

    private void BtnCreateGroupChat_Click(object sender, EventArgs e)
    {
        var dialog = new CreateGroupChatDialog(_currentUser, _friendController.Friends, AddChatPanel);
        dialog.ShowDialog();
    }

    private void AddChatPanel(ChatInfo chat)
    {
        var panel = new ChatPanelUC(
            chat,
            _currentUser.Id,
            (s, _) => MouseEventUtils.OnFriendPanelMove(s, p => ColorHelper.SetPanelColor(p, 35, 46, 60)),
            (s, _) => MouseEventUtils.OnFriendPanelMove(s, p => ColorHelper.SetPanelColor(p, 23, 33, 43)),
            (s, e) => _ = SafeInvoke(async () => await HandleChatClick(s, e))
        );
        chatListPanel.Controls.Add(panel);
    }

    private async Task HandleChatClick(object? sender, MouseEventArgs e)
    {
        var control = sender as Control;

        while (control != null && control is not ChatPanelUC)
            control = control.Parent;

        if (control is not ChatPanelUC panel) return;

        switch (e.Button)
        {
            case MouseButtons.Left:
                _chat = panel.Chat;
                lblLoadedChat.Text = _chat.Name;
                dialogPanel.Controls.Clear();
                await LoadDialogAsync();
                break;
            case MouseButtons.Right:
                ShowChatContextMenu(panel, e.Location);
                break;
            case MouseButtons.None:
            case MouseButtons.Middle:
            case MouseButtons.XButton1:
            case MouseButtons.XButton2:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ShowChatContextMenu(ChatPanelUC chatPanel, Point location)
    {
        CloseActiveContextMenu();

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
            ForeColor = Color.FromArgb(220, 80, 80),
            BorderRadius = 6,
            Font = new Font("Segoe UI", 9f),
            FillColor = Color.Transparent,
            HoverState = { FillColor = Color.FromArgb(45, 58, 71) }
        };

        var chat = chatPanel.Chat;

        var isAdmin = chat.Participants
            .Select(p => p.Role == ChatParticipationRole.Admin && p.UserId == _currentUser.Id)
            .FirstOrDefault();

        editButton.Visible = isAdmin;
        deleteButton.Location = isAdmin ? new Point(5, 42) : new Point(5, 5);
        menuPanel.Size = isAdmin ? new Size(160, 80) : new Size(160, 42);

        editButton.Click += (_, _) =>
        {
            CloseActiveContextMenu();
            _ = SafeInvoke(async () =>
            {
                /*var dialog = new InputDialog("Изменить чат", chat.Text);
                await dialog.ShowDialogAsync();
                if (dialog.Result == null) return;

                await _chatController.EditMessage(chat, dialog.Result);
                var panel = FindMessagePanel(chat.Id);
                if (panel != null) panel.UpdateText(dialog.Result);*/
            });
        };

        deleteButton.Click += (_, _) =>
        {
            CloseActiveContextMenu();
            _ = SafeInvoke(async () =>
            {
                await _chatController.DeletePrivateChat(chat, _currentUser.Id);
                chatListPanel.Controls.Remove(chatPanel);
            });
        };

        menuPanel.Controls.Add(editButton);
        menuPanel.Controls.Add(deleteButton);

        var formPos = PointToClient(chatPanel.PointToScreen(location));
        menuPanel.Location = formPos;
        Controls.Add(menuPanel);
        menuPanel.BringToFront();
        menuPanel.Focus();

        _activeMenuPanel = menuPanel;
        _menuFilter = new OutsideClickFilter(menuPanel, this, CloseActiveContextMenu);
        System.Windows.Forms.Application.AddMessageFilter(_menuFilter);
    }

    private async Task LoadDialogAsync()
    {
        _messagesPanel = new FlowLayoutPanel
        {
            Parent = dialogPanel,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true
        };

        chatTopPanel.Visible = true;
        btnSndMsg.Visible = true;
        txtBoxMessage.Visible = true;
        txtBoxPanel.Visible = true;

        if (_chat == null) return;
        await _chatController.LoadChat(_currentUser.Id, _chat.Id);
    }

    private void LoadMessageUI(MessageInfo message)
    {
        var panel = new MessagePanelUC(message, Message_MouseClick);
        _messagesPanel.Controls.Add(panel);
        _messagesPanel.ScrollControlIntoView(panel);
    }

    private void Message_MouseClick(object? sender, MouseEventArgs e)
    {
        if ((e.Button & MouseButtons.Right) == 0) return;

        var control = sender as Control;

        while (control != null && control is not MessagePanelUC)
            control = control.Parent;

        if (control is not MessagePanelUC panel) return;

        ShowMessageContextMenu(panel, e.Location);
    }

    private void ShowMessageContextMenu(MessagePanelUC messagePanel, Point location)
    {
        CloseActiveContextMenu();

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
            ForeColor = Color.FromArgb(220, 80, 80),
            BorderRadius = 6,
            Font = new Font("Segoe UI", 9f),
            FillColor = Color.Transparent,
            HoverState = { FillColor = Color.FromArgb(45, 58, 71) }
        };

        var message = messagePanel.Message;

        var isOwnMessage = message.Sender.Id == _currentUser.Id;

        editButton.Visible = isOwnMessage;
        deleteButton.Location = isOwnMessage ? new Point(5, 42) : new Point(5, 5);
        menuPanel.Size = isOwnMessage ? new Size(160, 80) : new Size(160, 42);

        editButton.Click += (_, _) =>
        {
            CloseActiveContextMenu();
            _ = SafeInvoke(async () =>
            {
                var dialog = new InputDialog("Изменить сообщение", message.Text);
                await dialog.ShowDialogAsync();
                if (dialog.Result == null) return;

                await _chatController.EditMessage(message, dialog.Result);
                messagePanel.UpdateText(dialog.Result);
                /*var panel = FindMessagePanel(message.Id);
                if (panel != null) panel.UpdateText(dialog.Result);*/
            });
        };

        deleteButton.Click += (_, _) =>
        {
            CloseActiveContextMenu();
            _ = SafeInvoke(async () =>
            {
                await _chatController.DeleteMessage(message);
                _messagesPanel.Controls.Remove(messagePanel);
                /*var panel = FindMessagePanel(message.Id);
                if (panel != null) _messagesPanel.Controls.Remove(panel);*/
            });
        };

        menuPanel.Controls.Add(editButton);
        menuPanel.Controls.Add(deleteButton);

        var formPos = PointToClient(messagePanel.PointToScreen(location));
        menuPanel.Location = formPos;
        Controls.Add(menuPanel);
        menuPanel.BringToFront();
        menuPanel.Focus();

        _activeMenuPanel = menuPanel;
        _menuFilter = new OutsideClickFilter(menuPanel, this, CloseActiveContextMenu);
        System.Windows.Forms.Application.AddMessageFilter(_menuFilter);
    }

    private void CloseActiveContextMenu()
    {
        if (_menuFilter != null)
        {
            System.Windows.Forms.Application.RemoveMessageFilter(_menuFilter);
            _menuFilter = null;
        }

        if (_activeMenuPanel is not { IsDisposed: false }) return;

        _activeMenuPanel.Dispose();
        _activeMenuPanel = null;
    }

    private void btnSndMsg_Click(object sender, EventArgs e)
    {
        _ = SafeInvoke(EnqueueMessageAsync);
    }

    private void txtBoxMessage_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        e.SuppressKeyPress = true;
        e.Handled = true;
        _ = SafeInvoke(EnqueueMessageAsync);
    }

    private async Task EnqueueMessageAsync()
    {
        var text = txtBoxMessage.Text.Trim();

        if (string.IsNullOrWhiteSpace(text)) return;
        if (_chatController.CurrentChat == null) return;

        if (_chat == null) return;

        txtBoxMessage.Clear();

        var message = new MessageInfo
        {
            Text = text,
            Sender = _currentUser,
            ChatId = _chatController.CurrentChat.Id,
            TempId = Guid.NewGuid()
        };

        LoadMessageUI(message);

        await _messageQueue!.EnqueueAsync(new OutgoingChatMessage(
            _chatController.CurrentChat.Id,
            _currentUser.Id,
            text,
            message.TempId));
    }

    #endregion

    #region Profile

    private void CreateProfilePanel()
    {
        _profilePanel = new ProfilePanelUC(
            _currentUser,
            mainPanel.Width + leftPanel.Width,
            Height,
            OpenCloseProfile,
            AddFriendTxtBox_KeyDown,
            updatedUser =>
            {
                _currentUser = updatedUser;
                _profilePanel.UpdateUsername(_currentUser.Username);
            }
        );

        Controls.Add(_profilePanel);
    }

    private void btnOpenProfile_Click(object sender, EventArgs e)
    {
        OpenCloseProfile();
    }

    private void OpenCloseProfile()
    {
        switch (_profilePanel.Visible)
        {
            case false:
                _profilePanel.Visible = true;
                leftPanel.Visible = false;
                chatListPanel.Visible = false;
                mainPanel.Visible = false;
                searchPanel.Visible = false;
                txtBoxSearch.Visible = false;
                break;
            case true:
                _profilePanel.Visible = false;
                leftPanel.Visible = true;
                chatListPanel.Visible = true;
                mainPanel.Visible = true;
                searchPanel.Visible = true;
                txtBoxSearch.Visible = true;
                break;
        }
    }

    #endregion

    #region Utils

    private MessagePanelUC? FindMessagePanel(int messageId)
    {
        return _messagesPanel.Controls
            .OfType<MessagePanelUC>()
            .FirstOrDefault(p => p.Message.Id == messageId);
    }

    private ChatPanelUC? FindPrivateChatPanel(int chatId)
    {
        return chatListPanel.Controls
            .OfType<ChatPanelUC>()
            .FirstOrDefault(p => p.Tag is ChatInfo { Type: ChatType.Private } c && c.Id == chatId);
    }

    public void btnUpdServers_Click(object sender, EventArgs e)
    {
        /* todo */
    }

    #endregion
}