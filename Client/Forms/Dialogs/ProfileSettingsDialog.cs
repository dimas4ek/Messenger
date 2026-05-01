using Application.DTO;
using Application.Utils;
using Client.Api.Clients;
using Client.Services;
using Client.UI.Utils;
using Client.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Forms.Dialogs;

public partial class ProfileSettingsDialog : Form
{
    private readonly UserInfo _currentUser;
    private readonly IDialogService _dialogService;

    private readonly Action<UserInfo> _onUserUpdated;
    private readonly UserApiClient _userApiClient;

    public ProfileSettingsDialog(UserInfo currentUser, Action<UserInfo> onUserUpdated)
    {
        _currentUser = currentUser;
        _onUserUpdated = onUserUpdated;

        _dialogService = App.Services.GetRequiredService<IDialogService>();
        _userApiClient = App.Services.GetRequiredService<UserApiClient>();

        InitializeComponent();

        Load += (_, _) =>
        {
            avatar.Image = ImageHelper.GetAvatar(_currentUser.Avatar);
            avatar.Click += ChangeAvatar;

            usernameValueLabel.Text = _currentUser.Username;

            editUsernameButton.Click += EditUsername;
            editPasswordButton.Click += EditPassword;
        };
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

        usernameValueLabel.Text = _currentUser.Username;

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

        avatar.Image = Image.FromFile(filePath);
        avatar.SizeMode = PictureBoxSizeMode.Zoom;
        _currentUser.Avatar = resultValue.User.Avatar;
    }
}