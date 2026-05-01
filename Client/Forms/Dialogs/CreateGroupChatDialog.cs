using Application.DTO;
using Application.Utils;
using Client.Api.Clients;
using Client.Properties;
using Client.Services;
using Client.UI.UserControls;
using Client.UI.Utils;
using Client.Utils;
using Guna.UI2.WinForms;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Forms.Dialogs;

public partial class CreateGroupChatDialog : Form
{
    private readonly ChatApiClient _chatApiClient;
    private readonly UserInfo _currentUser;
    private readonly IDialogService _dialogService;
    private readonly Action<ChatInfo> _onChatCreated;

    private readonly List<UserInfo> _selectedUsers = [];
    private readonly UtilsApiClient _utilsApiClient;

    private ImageInfo? _image;

    public CreateGroupChatDialog(UserInfo currentUser, IEnumerable<UserInfo> friends, Action<ChatInfo> onChatCreated)
    {
        _currentUser = currentUser;
        _onChatCreated = onChatCreated;

        _dialogService = App.Services.GetRequiredService<IDialogService>();
        _chatApiClient = App.Services.GetRequiredService<ChatApiClient>();
        _utilsApiClient = App.Services.GetRequiredService<UtilsApiClient>();

        InitializeComponent();

        Load += (_, _) =>
        {
            groupImage.Image = GetImage();
            groupImage.Click += AddGroupImage;
            createButton.Click += HandleCreateGroupChat;

            foreach (var friend in friends)
                AddFriendPanel(friend);
        };
    }

    private string GroupName => groupNameTextBox.Text;
    private IReadOnlyList<int> SelectedUsers => _selectedUsers.Select(u => u.Id).ToList();

    private void AddFriendPanel(UserInfo friend)
    {
        var panel = new FriendSelectPanelUC(
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
                if (_selectedUsers.Remove(user))
                {
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

        friendsPanel.Controls.Add(panel);
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

        groupImage?.Image = Image.FromFile(filePath);
        groupImage?.SizeMode = PictureBoxSizeMode.Zoom;

        _image = imageResponse.Image;
    }

    private Image GetImage()
    {
        if (_image?.Data == null) return Resources.DefaultAvatarImage;

        using var ms = new MemoryStream(_image.Data);
        return Image.FromStream(ms);
    }
}