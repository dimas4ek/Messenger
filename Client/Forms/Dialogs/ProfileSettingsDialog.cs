using Application.DTO;
using Application.Utils;
using Client.Api.Clients;
using Client.Properties;
using Client.Services;
using Client.Utils;
using Guna.UI2.WinForms;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Forms.Dialogs;

public class ProfileSettingsDialog : Form
{
    private readonly UserInfo _currentUser;
    private readonly IDialogService _dialogService;

    private readonly Action<UserInfo> _onUserUpdated;
    private readonly UserApiClient _userApiClient;

    private Guna2CirclePictureBox _avatar;
    private Label? _passwordValueLabel;
    private Label? _usernameValueLabel;

    public ProfileSettingsDialog(UserInfo currentUser, Action<UserInfo> onUserUpdated)
    {
        _currentUser = currentUser;
        _onUserUpdated = onUserUpdated;

        _dialogService = App.Services.GetRequiredService<IDialogService>();
        _userApiClient = App.Services.GetRequiredService<UserApiClient>();

        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Size = new Size(400, 420);
        StartPosition = FormStartPosition.CenterParent;
        Text = "Profile Settings";
        BackColor = Color.FromArgb(23, 33, 43);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        _avatar = new Guna2CirclePictureBox
        {
            Size = new Size(100, 100),
            Location = new Point((Width - 100) / 2 - 10, 20),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = GetAvatar(),
            BackColor = Color.Transparent
        };
        _avatar.Click += ChangeAvatar;
        Controls.Add(_avatar);

        var usernameLabel = new Label
        {
            Text = "Username",
            ForeColor = Color.Gray,
            Location = new Point(30, 150),
            AutoSize = true
        };
        Controls.Add(usernameLabel);

        _usernameValueLabel = new Label
        {
            Text = _currentUser.Username,
            ForeColor = Color.White,
            Location = new Point(30, 175),
            AutoSize = true,
            Font = new Font("Segoe UI", 11, FontStyle.Bold)
        };
        Controls.Add(_usernameValueLabel);

        var editUsernameButton = new Guna2Button
        {
            Text = "Изменить",
            Location = new Point(260, 170),
            Size = new Size(100, 30),
            FillColor = Color.FromArgb(45, 140, 240)
        };
        editUsernameButton.Click += EditUsername;
        Controls.Add(editUsernameButton);

        var passwordLabel = new Label
        {
            Text = "Password",
            ForeColor = Color.Gray,
            Location = new Point(30, 230),
            AutoSize = true
        };
        Controls.Add(passwordLabel);

        _passwordValueLabel = new Label
        {
            Text = "********",
            ForeColor = Color.White,
            Location = new Point(30, 255),
            AutoSize = true,
            Font = new Font("Segoe UI", 11, FontStyle.Bold)
        };
        Controls.Add(_passwordValueLabel);

        var editPasswordButton = new Guna2Button
        {
            Text = "Изменить",
            Location = new Point(260, 250),
            Size = new Size(100, 30),
            FillColor = Color.FromArgb(45, 140, 240)
        };
        editPasswordButton.Click += EditPassword;
        Controls.Add(editPasswordButton);
    }

    private async void EditUsername(object? sender, EventArgs e)
    {
        var dialog = new InputDialog(
            "Изменить имя",
            _currentUser.Username,
            "Введите новое имя");

        if (await dialog.ShowDialogAsync() != DialogResult.OK ||
            string.IsNullOrWhiteSpace(dialog.Result)) return;

        var result = await _userApiClient.UpdateUsername(_currentUser.Id, dialog.Result);

        if (!result.IsSuccess || result.Value == null)
        {
            _dialogService.ShowError(result.ToMessage());
            return;
        }

        _currentUser.Username = result.Value.User.Username;

        _usernameValueLabel?.Text = _currentUser.Username;

        _onUserUpdated(_currentUser);
    }

    private async void EditPassword(object? sender, EventArgs e)
    {
        var dialog = new InputDialog(
            "Изменить пароль",
            "",
            "Введите новый пароль",
            true);

        if (await dialog.ShowDialogAsync() != DialogResult.OK ||
            string.IsNullOrWhiteSpace(dialog.Result)) return;

        var result = await _userApiClient.UpdatePassword(_currentUser.Id, dialog.Result);

        if (!result.IsSuccess || result.Value == null) _dialogService.ShowError(result.ToMessage());
    }


    // Открывает системный диалог выбора файла изображения
    // в отдельном STA-потоке и возвращает путь к выбранному файлу.
    //
    // Почему используется StaTaskScheduler:
    // OpenFileDialog требует поток с ApartmentState.STA.
    // Если вызвать диалог из обычного Task.Run / ThreadPool,
    // поток будет MTA и диалог может завершиться ошибкой.
    //
    // Почему используется Task.Factory.StartNew:
    // Позволяет указать собственный TaskScheduler,
    // чтобы задача выполнилась именно в STA-потоке.
    //
    // Результат:
    // - путь к выбранному файлу, если пользователь нажал OK
    // - null, если пользователь отменил выбор
    private async void ChangeAvatar(object? sender, EventArgs e)
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

        var result = await _userApiClient.ChangeAvatar(_currentUser.Id, name, imageBytes, contentType);

        var resultValue = result.Value;

        if (!result.IsSuccess || resultValue == null)
        {
            _dialogService.ShowError(result.ToMessage());
            return;
        }

        _avatar.Image = Image.FromFile(filePath);
        _avatar.SizeMode = PictureBoxSizeMode.Zoom;
        _currentUser.Avatar = resultValue.User.Avatar;
    }

    private Image GetAvatar()
    {
        if (_currentUser.Avatar?.Data == null) return Resources.DefaultAvatar;

        using var ms = new MemoryStream(_currentUser.Avatar.Data);
        return Image.FromStream(ms);
    }
}