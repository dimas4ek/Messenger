using Application.DTO;
using Application.Utils;
using Client.Api.Clients;
using Client.Properties;
using Client.Services;
using Client.UI.Factory;
using Client.UI.Utils;
using Client.Utils;
using Guna.UI2.WinForms;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Forms.Dialogs;

public class CreateGroupChatDialog : Form
{
    private readonly ChatApiClient _chatApiClient;
    private readonly UserInfo _currentUser;
    private readonly IDialogService _dialogService;
    private readonly IEnumerable<UserInfo> _friends;
    private readonly Action<ChatInfo> _onChatCreated;

    private readonly List<UserInfo> _selectedUsers = [];
    private readonly UtilsApiClient _utilsApiClient;
    private FlowLayoutPanel _friendsPanel = null!;
    private Guna2CirclePictureBox? _groupImage;
    private Guna2TextBox _groupNameTextBox = null!;

    private ImageInfo? _image;

    public CreateGroupChatDialog(UserInfo currentUser, IEnumerable<UserInfo> friends, Action<ChatInfo> onChatCreated)
    {
        _currentUser = currentUser;
        _friends = friends;
        _onChatCreated = onChatCreated;

        _dialogService = App.Services.GetRequiredService<IDialogService>();
        _chatApiClient = App.Services.GetRequiredService<ChatApiClient>();
        _utilsApiClient = App.Services.GetRequiredService<UtilsApiClient>();

        InitializeComponent();
    }

    private string GroupName => _groupNameTextBox.Text;
    private IReadOnlyList<int> SelectedUsers => _selectedUsers.Select(u => u.Id).ToList();

    private void InitializeComponent()
    {
        CreateGroupChatPanel();

        foreach (var friend in _friends)
            AddFriendPanel(friend);
    }

    private void CreateGroupChatPanel()
    {
        Size = new Size(400, 500);
        StartPosition = FormStartPosition.CenterParent;
        Text = "Create Group Chat";
        BackColor = Color.FromArgb(23, 33, 43);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        _groupImage = new Guna2CirclePictureBox
        {
            Size = new Size(100, 100),
            Location = new Point((Width - 100) / 2 - 10, 10),
            SizeMode = PictureBoxSizeMode.StretchImage,
            Image = GetImage(),
            BackColor = Color.Transparent
        };
        _groupImage.Click += AddGroupImage;
        Controls.Add(_groupImage);

        var nameLabel = new Label();
        nameLabel.Text = "Group name";
        nameLabel.ForeColor = Color.White;
        nameLabel.Location = new Point(20, 120);
        nameLabel.AutoSize = true;

        _groupNameTextBox = new Guna2TextBox();
        _groupNameTextBox.Size = new Size(360, 36);
        _groupNameTextBox.Location = new Point(20, 145);
        _groupNameTextBox.PlaceholderText = "Enter group name...";
        _groupNameTextBox.FillColor = Color.FromArgb(35, 46, 60);
        _groupNameTextBox.ForeColor = Color.White;
        _groupNameTextBox.BorderColor = Color.FromArgb(55, 66, 80);

        var friendsLabel = new Label();
        friendsLabel.Text = "Add friends";
        friendsLabel.ForeColor = Color.White;
        friendsLabel.Location = new Point(20, 205);
        friendsLabel.AutoSize = true;

        _friendsPanel = new FlowLayoutPanel();
        _friendsPanel.Location = new Point(20, 230);
        _friendsPanel.Size = new Size(360, 180);
        _friendsPanel.FlowDirection = FlowDirection.TopDown;
        _friendsPanel.WrapContents = false;
        _friendsPanel.AutoScroll = true;
        _friendsPanel.BackColor = Color.FromArgb(23, 33, 43);
        _friendsPanel.Layout += (s, e) =>
        {
            _friendsPanel.HorizontalScroll.Visible = false;
            _friendsPanel.HorizontalScroll.Enabled = false;
            _friendsPanel.AutoScrollMinSize = _friendsPanel.AutoScrollMinSize with { Width = 0 };
        };

        var createButton = new Guna2Button();
        createButton.Text = "Create";
        createButton.Size = new Size(360, 36);
        createButton.Location = new Point(
            (ClientSize.Width - createButton.Width) / 2, 420);
        createButton.FillColor = Color.FromArgb(114, 137, 218);
        createButton.ForeColor = Color.White;
        createButton.Click += HandleCreateGroupChat;

        Controls.AddRange(nameLabel, _groupNameTextBox, friendsLabel, _friendsPanel, createButton);
    }

    private void AddFriendPanel(UserInfo friend)
    {
        var panel = FriendSelectPanelFactory.Create(
            friend,
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
            (s, _) =>
            {
                if (s is not Guna2Button btn || btn.Tag is not UserInfo user) return;
                if (_selectedUsers.Contains(user))
                {
                    _selectedUsers.Remove(user);
                    btn.Text = "Add";
                    btn.FillColor = Color.FromArgb(114, 137, 218);
                }
                else
                {
                    _selectedUsers.Add(user);
                    btn.Text = "Added";
                    btn.FillColor = Color.FromArgb(80, 80, 80);
                }
            }
        );

        _friendsPanel.Controls.Add(panel);
    }

    private async void HandleCreateGroupChat(object? s, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(GroupName)) return;
        if (SelectedUsers.Count == 0) return;

        var result = await _chatApiClient.CreateGroupChat(GroupName, _image?.Id, _currentUser.Id, SelectedUsers);
        if (!result.IsSuccess || result.Value == null)
        {
            _dialogService.ShowError(result.ToMessage());
            return;
        }

        _onChatCreated?.Invoke(result.Value.Chat);

        Dispose();
    }

    private async void AddGroupImage(object? sender, EventArgs e)
    {
        var filePath = await Task.Factory.StartNew(() =>
            {
                using var dialog = new OpenFileDialog();
                dialog.Filter = "Images|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.webp";
                dialog.Title = "Выберите изображение";

                return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
            },
            CancellationToken.None,
            TaskCreationOptions.None,
            new StaTaskScheduler());

        if (filePath == null) return;

        var imageBytes = await File.ReadAllBytesAsync(filePath);
        var name = Path.GetFileNameWithoutExtension(filePath);
        var contentType = ImageContentTypeExtensions.FromExtension(filePath);

        var result = await _utilsApiClient.AddImage(name, imageBytes, contentType);

        var imageResponse = result.Value;

        if (!result.IsSuccess || imageResponse == null)
        {
            _dialogService.ShowError(result.ToMessage());
            return;
        }

        _groupImage?.Image = Image.FromFile(filePath);
        _groupImage?.SizeMode = PictureBoxSizeMode.Zoom;

        _image = imageResponse.Image;
    }

    private Image GetImage()
    {
        if (_image?.Data == null) return Resources.DefaultAvatarImage;

        using var ms = new MemoryStream(_image.Data);
        return Image.FromStream(ms);
    }
}