using Application.DTO;
using Client.Api.Clients;
using Client.Api.Realtime;
using Client.Controllers;
using Client.Filter;
using Client.Forms.Base;
using Client.Forms.Dialogs;
using Client.Properties;
using Client.Services;
using Client.UI;
using Contracts.DTO.Chat;
using Guna.UI2.WinForms;
using LibVLCSharp.Shared;
using System.Threading.Channels;
using static Client.Services.MessageQueueService;

namespace Client.Forms;

public partial class ClientForm : BaseForm
{
    private readonly ChatController _chatController = null!;
    private readonly FriendController _friendController = null!;
    private readonly ProfileController _profileController = null!;
    private readonly UserContext _userContext = null!;

    private MessageQueueService? _messageQueue;
    private readonly ChatApiClient _chatApiClient = null!;

    private UserInfo _currentUser = null!;
    private UserInfo? _companion;

    private FlowLayoutPanel _chatPanel = null!;
    private Guna2Panel _profilePanel = null!;
    private Guna2TextBox _addFriendTxtBox = null!;

    private bool _isFriendTxtBoxOpen;
    private bool _isProfileOpen;

    private Guna2Panel? _activeMenuPanel;
    private OutsideClickFilter? _menuFilter;

    public ClientForm() : base()
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

        _chatController.MessageLoaded += OnMessageLoaded;
        _chatController.MessageReceived += OnMessageReceived;
        _chatController.MessageUpdated += OnMessageUpdated;
        _chatController.MessageDeleted += OnMessageDeleted;
    }

    #region Load / Close

    public async void ClientForm_Load(object? sender, EventArgs e)
    {
        _ = SafeInvoke(async () =>
        {
            _currentUser = _userContext.CurrentUser ?? throw new Exception("Пользователь не авторизован");

            DoImportantThings();
            CreateProfilePanel();

            await _chatController.ConnectAsync(_currentUser.Id);

            var friends = await _friendController.LoadFriendsAsync(_currentUser.Id);
            foreach (var friend in friends)
                AddFriendPanel(friend);
            lblLoadingChats.Visible = false;

            _messageQueue = new MessageQueueService(_chatApiClient, DialogService);
            _messageQueue.MessageConfirmed += OnMessageConfirmed;
            _messageQueue.Start();
        });
    }

    private async void ClientForm_Close(object sender, FormClosingEventArgs e)
    {
        _ = SafeInvoke(async () =>
        {
            await _chatController.DisconnectAsync();

            if (_messageQueue != null)
                await _messageQueue.DisposeAsync();

            await _profileController.LogoutAsync();

            Environment.Exit(0);
        });
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

    #endregion

    #region Chat Events

    private void OnMessageLoaded(MessageInfo message)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => OnMessageLoaded(message));
            return;
        }

        //if (message.Sender.Id == _currentUser.Id) return;

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

        MessagePanelFactory.UpdateText(panel, message.Text);
        ((MessageInfo)panel.Tag!).Text = message.Text;
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

        _chatPanel.Controls.Remove(panel);
    }

    private void OnMessageConfirmed(OutgoingChatMessage item, MessageInfo message)
    {
        BeginInvoke(() =>
        {
            var panel = _chatPanel.Controls
                .OfType<Guna2Panel>()
                .FirstOrDefault(p => p.Tag is MessageInfo m && m.TempId == item.TempId);

            if (panel == null) return;

            message.TempId = item.TempId;
            panel.Tag = message;
        });
    }

    #endregion

    #region Friends

    private void AddFriendPanel(UserInfo friend)
    {
        var panel = FriendPanelFactory.Create(
            friend,
            onMove: (s, _) =>
            {
                if (s is not Control c) return;

                c.Cursor = Cursors.Hand;
                FriendPanelFactory.SetColor((Guna2Panel)(c is Label l ? l.Parent! : c), 35, 46, 60);
            },
            onLeave: (s, _) =>
            {
                if (s is Control c) FriendPanelFactory.SetColor((Guna2Panel)(c is Label l ? l.Parent! : c), 23, 33, 43);
            },
            onClick: (s, e) => _ = SafeInvoke(() => HandleFriendClick(s))
        );
        addedFriendPanel.Controls.Add(panel);
    }

    private async Task HandleFriendClick(object? sender)
    {
        var user = sender switch
        {
            Guna2Panel p => p.Tag as UserInfo,
            Label l => l.Tag as UserInfo,
            _ => null
        };

        if (user == null || (_companion?.Id == user.Id)) return;

        _companion = user;
        lblCompanionUsername.Text = _companion.Username;
        chatPanelGuna.Controls.Clear();

        await LoadDialogAsync();
    }

    private async void AddFriendTxtBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        _ = SafeInvoke(async () =>
        {
            await _friendController.AddFriendAsync(
                _currentUser.Id,
                _currentUser.Username,
                _addFriendTxtBox.Text);

            addedFriendPanel.Controls.Clear();
            foreach (var friend in _friendController.Friends)
                AddFriendPanel(friend);

            _addFriendTxtBox.Clear();
        });
    }

    private async void txtBoxSearch_KeyUp(object sender, KeyEventArgs e)
    {
        _ = SafeInvoke(() =>
        {
            var results = string.IsNullOrWhiteSpace(txtBoxSearch.Text)
                ? _friendController.Friends
                : _friendController.Search(txtBoxSearch.Text, _currentUser.Username);

            addedFriendPanel.Controls.Clear();
            foreach (var friend in results)
                AddFriendPanel(friend);
            return Task.CompletedTask;
        });
    }

    #endregion

    #region Chat

    private async Task LoadDialogAsync()
    {
        _chatPanel = new FlowLayoutPanel
        {
            Parent = chatPanelGuna,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true
        };

        companionPanel.Visible = true;
        btnSndMsg.Visible = true;
        txtBoxMessage.Visible = true;
        txtBoxPanel.Visible = true;

        if (_companion == null) return;

        await _chatController.LoadDialogAsync(_currentUser.Id, _companion);
    }

    private void LoadMessageUI(MessageInfo message)
    {
        var panel = MessagePanelFactory.Create(message, MessagePanel_MouseClick);
        _chatPanel.Controls.Add(panel);
        _chatPanel.ScrollControlIntoView(panel);
    }

    private void MessagePanel_MouseClick(object? sender, MouseEventArgs e)
    {
        if ((e.Button & MouseButtons.Right) == 0) return;

        var panel = (Guna2Panel)sender!;
        var message = (MessageInfo)panel.Tag!;
        ShowMessageContextMenu(panel, message, e.Location);
    }

    private void ShowMessageContextMenu(Guna2Panel messagePanel, MessageInfo message, Point location)
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

        var isOwnMessage = message.Sender.Id == _currentUser.Id;

        editButton.Visible = isOwnMessage;
        deleteButton.Location = isOwnMessage ? new Point(5, 42) : new Point(5, 5);
        menuPanel.Size = isOwnMessage ? new Size(160, 80) : new Size(160, 42);

        editButton.Click += (_, _) =>
        {
            menuPanel.Hide();
            _ = SafeInvoke(async () =>
            {
                var dialog = new EditMessageDialog(message.Text);
                await dialog.ShowDialogAsync();
                if (dialog.Result == null) return;

                await _chatController.EditAsync(message, dialog.Result);
                var panel = FindMessagePanel(message.Id);
                if (panel != null) MessagePanelFactory.UpdateText(panel, dialog.Result);
            });
            menuPanel.Dispose();
        };

        deleteButton.Click += (_, _) =>
        {
            menuPanel.Hide();
            _ = SafeInvoke(async () =>
            {
                await _chatController.DeleteAsync(message);
                var panel = FindMessagePanel(message.Id);
                if (panel != null) _chatPanel.Controls.Remove(panel);
            });
            menuPanel.Dispose();
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

    private void btnSndMsg_Click(object sender, EventArgs e) =>
        _ = SafeInvoke(EnqueueMessageAsync);

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
        if (string.IsNullOrWhiteSpace(text) || _companion == null || _chatController.CurrentChat == null) return;

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
        _profilePanel = ProfilePanelFactory.Create(
            _currentUser.Username,
            mainPanel.Width + leftPanel.Width,
            Height,
            CloseProfile,
            AddFriendButton_Click
        );

        Controls.Add(_profilePanel);

        _addFriendTxtBox = new Guna2TextBox { Visible = false };
    }

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

    private void btnOpenProfile_Click(object sender, EventArgs e) => OpenProfile();

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

    #region Utils

    private Guna2Panel? FindMessagePanel(int messageId) =>
        _chatPanel.Controls
            .OfType<Guna2Panel>()
            .FirstOrDefault(p => p.Tag is MessageInfo m && m.Id == messageId);

    public void btnUpdServers_Click(object sender, EventArgs e)
    {
        /* todo */
    }

    #endregion
}