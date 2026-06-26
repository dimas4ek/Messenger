using Application.DTO;
using Application.Utils;
using Client.Api.Clients;
using Client.Properties;
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
        LanguageManager.LanguageChanged += ApplyLocalization;
        ApplyLocalization();

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
            Strings.ProfileSettingsDialog_ChangeName,
            _currentUser.Username,
            Strings.ProfileSettingsDialog_EnterNewName);

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
            Strings.ProfileSettingsDialog_ChangePassword,
            "",
            Strings.ProfileSettingsDialog_EnterNewPassword,
            true);

        if (await dialog.ShowDialogAsync() != DialogResult.OK ||
            string.IsNullOrWhiteSpace(dialog.Result)) return;

        var result = await _userApiClient.UpdatePassword(_currentUser.Id, dialog.Result);

        if (!result.IsSuccess || result.Value == null) _dialogService.ShowError(result.ToMessage());
    }
    
    private async void ChangeAvatar(object? sender, EventArgs e)
    {
        var file = await ImageHelper.OpenFile();

        if (file == null) return;

        var imageBytes = await File.ReadAllBytesAsync(file);
        var name = Path.GetFileNameWithoutExtension(file);
        var contentType = ImageContentTypeExtensions.FromExtension(file);

        var result = await _userApiClient.ChangeAvatar(_currentUser.Id, name, imageBytes, contentType);

        var resultValue = result.Value;

        if (!result.IsSuccess || resultValue == null)
        {
            _dialogService.ShowError(result.ToMessage());
            return;
        }

        avatar.Image = Image.FromFile(file);
        avatar.SizeMode = PictureBoxSizeMode.Zoom;
        _currentUser.Avatar = resultValue.User.Avatar;
    }
    
    #region Language
    
    private void ApplyLocalization()
    {
        usernameLabel.Text = Strings.ProfileSettingsDialog_Username;
        passwordLabel.Text = Strings.ProfileSettingsDialog_Password;
        editUsernameButton.Text = Strings.ProfileSettingsDialog_Edit;
        editPasswordButton.Text = Strings.ProfileSettingsDialog_Edit;
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        LanguageManager.LanguageChanged -= ApplyLocalization;
        base.OnFormClosed(e);
    }
    
    #endregion
}