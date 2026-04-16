using Application.DTO;
using Client.Api.Clients;
using Client.Forms.Dialogs;
using Client.Services;
using Client.Utils;
using Guna.UI2.WinForms;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Forms;

public class ProfileSettingsDialog : Form
{
    private readonly IDialogService _dialogService;
    private readonly UserInfo _user;
    private readonly UserApiClient _userApiClient;
    private Label passwordValueLabel;
    private Label usernameValueLabel;

    public ProfileSettingsDialog(UserInfo user)
    {
        _dialogService = App.Services.GetRequiredService<IDialogService>();
        _userApiClient = App.Services.GetRequiredService<UserApiClient>();
        _user = user;

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

        // 🖼 Аватар
        var avatar = new Guna2CirclePictureBox
        {
            Size = new Size(100, 100),
            Location = new Point((Width - 100) / 2 - 10, 20),
            SizeMode = PictureBoxSizeMode.Zoom,
            //Image = Properties.Resources.default_avatar,
            BackColor = Color.Transparent
        };
        Controls.Add(avatar);

        // 👤 USERNAME
        var usernameLabel = new Label
        {
            Text = "Username",
            ForeColor = Color.Gray,
            Location = new Point(30, 150),
            AutoSize = true
        };
        Controls.Add(usernameLabel);

        usernameValueLabel = new Label
        {
            Text = _user.Username,
            ForeColor = Color.White,
            Location = new Point(30, 175),
            AutoSize = true,
            Font = new Font("Segoe UI", 11, FontStyle.Bold)
        };
        Controls.Add(usernameValueLabel);

        var editUsernameButton = new Guna2Button
        {
            Text = "Изменить",
            Location = new Point(260, 170),
            Size = new Size(100, 30),
            FillColor = Color.FromArgb(45, 140, 240)
        };
        editUsernameButton.Click += EditUsername;
        Controls.Add(editUsernameButton);

        // 🔒 PASSWORD
        var passwordLabel = new Label
        {
            Text = "Password",
            ForeColor = Color.Gray,
            Location = new Point(30, 230),
            AutoSize = true
        };
        Controls.Add(passwordLabel);

        passwordValueLabel = new Label
        {
            Text = "********",
            ForeColor = Color.White,
            Location = new Point(30, 255),
            AutoSize = true,
            Font = new Font("Segoe UI", 11, FontStyle.Bold)
        };
        Controls.Add(passwordValueLabel);

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

    // 👤 Изменение имени
    private async void EditUsername(object? sender, EventArgs e)
    {
        var dialog = new InputDialog(
            "Изменить имя",
            _user.Username,
            "Введите новое имя");

        if (await dialog.ShowDialogAsync() != DialogResult.OK ||
            string.IsNullOrWhiteSpace(dialog.Result)) return;

        var result = await _userApiClient.UpdateUsername(_user.Id, dialog.Result);

        if (!result.IsSuccess || result.Value == null)
        {
            _dialogService.ShowError(result.ToMessage());
            return;
        }

        usernameValueLabel.Text = dialog.Result;
    }

    // 🔒 Изменение пароля
    private async void EditPassword(object? sender, EventArgs e)
    {
        var dialog = new InputDialog(
            "Изменить пароль",
            "",
            "Введите новый пароль",
            true);

        if (await dialog.ShowDialogAsync() != DialogResult.OK ||
            string.IsNullOrWhiteSpace(dialog.Result)) return;

        var result = await _userApiClient.UpdatePassword(_user.Id, dialog.Result);

        if (!result.IsSuccess || result.Value == null)
        {
            _dialogService.ShowError(result.ToMessage());
            return;
        }

        usernameValueLabel.Text = "******";
    }
}