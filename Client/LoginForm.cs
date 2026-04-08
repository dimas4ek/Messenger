using Client.ApiClients;
using Client.Service;
using Client.Utils;
using Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Client;

public partial class LoginForm : Form
{
    private readonly AuthApiClient _authApiClient;
    private readonly IDialogService _dialogService;
    private readonly IServiceProvider _serviceProvider;
    private readonly UserContext _userContext;
    private LoginSwitch _mode = LoginSwitch.Login;

    public LoginForm(
        IServiceProvider serviceProvider,
        IDialogService dialogService, AuthApiClient authApiClient, UserContext userContext)
    {
        _serviceProvider = serviceProvider;
        _dialogService = dialogService;
        _authApiClient = authApiClient;
        _userContext = userContext;

        InitializeComponent();
        InitializeUI();
    }

    #region Main Things

    private void InitializeUI()
    {
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = true;

        Resize += (_, _) =>
        {
            if (WindowState != FormWindowState.Minimized)
                CenterPanel();
        };

        txtPassword.PasswordChar = '*';

        SetMode(LoginSwitch.Login);
        CenterPanel();

        Load += async (_, _) => await ChangeColor();
    }

    private void CenterPanel()
    {
        loginBackPanel.Left = (ClientSize.Width - loginBackPanel.Width) / 2;
        loginBackPanel.Top = (ClientSize.Height - loginBackPanel.Height) / 2;
    }

    private async Task ChangeColor()
    {
        int r = 0, g = 50, b = 50;

        BackColor = Color.FromArgb(r, g, b);

        while (true)
        {
            for (var i = 0; i < 50; i++)
            {
                r++;
                g--;
                BackColor = Color.FromArgb(r, g, b);
                await Task.Delay(100);
            }

            for (var i = 0; i < 50; i++)
            {
                g++;
                b--;
                BackColor = Color.FromArgb(r, g, b);
                await Task.Delay(100);
            }

            for (var i = 0; i < 50; i++)
            {
                b++;
                r--;
                BackColor = Color.FromArgb(r, g, b);
                await Task.Delay(100);
            }
        }
    }

    #endregion

    #region Auth

    private async void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            await ProcessAuth();
        }
        catch (Exception ex)
        {
            _dialogService.ShowError(ex.Message);
        }
    }

    private async void txtPassword_KeyUp(object sender, KeyEventArgs e)
    {
        try
        {
            if (e.KeyCode == Keys.Enter)
                await ProcessAuth();
        }
        catch (Exception ex)
        {
            _dialogService.ShowError(ex.Message);
        }
    }

    private void btnRegister_Click(object sender, EventArgs e)
    {
        SetMode(_mode == LoginSwitch.Login
            ? LoginSwitch.Registration
            : LoginSwitch.Login);
    }

    private async Task ProcessAuth()
    {
        var username = txtUsername.Text.Trim();
        var password = txtPassword.Text.Trim();

        if (!ValidateInput(username, password))
            return;

        ToggleUI(false);

        try
        {
            if (_mode == LoginSwitch.Login)
                await HandleLogin(username, password);
            else
                await HandleRegistration(username, password);
        }
        catch (Exception ex)
        {
            _dialogService.ShowError($"Ошибка: {ex.Message}");
        }
        finally
        {
            ToggleUI(true);
        }
    }

    private async Task HandleLogin(string username, string password)
    {
        var result = await _authApiClient.Login(username, password);

        if (!result.IsSuccess || result.Value == null)
        {
            _dialogService.ShowError(result.ToMessage());
            return;
        }

        var user = result.Value.User;

        if (user == null)
        {
            _dialogService.ShowError("Пользователь не найден");
            return;
        }

        user.Status = UserStatus.Online;

        _userContext.SetUser(user);

        OpenClientForm();
    }

    private async Task HandleRegistration(string username, string password)
    {
        var result = await _authApiClient.Register(username, password);

        if (!result.IsSuccess)
        {
            _dialogService.ShowError(result.ToMessage());
            return;
        }

        _dialogService.ShowMessage("Регистрация успешна! Теперь можете войти.");

        SetMode(LoginSwitch.Login);
        ClearInputs();
    }

    private void OpenClientForm()
    {
        var client = _serviceProvider.GetRequiredService<ClientForm>();
        client.Show();
        Hide();
    }

    #endregion

    #region Utils

    private void ToggleUI(bool enabled)
    {
        btnLogin.Enabled = enabled;
        btnRegister.Enabled = enabled;
    }

    private bool ValidateInput(string username, string password)
    {
        if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password)) return true;
        _dialogService.ShowMessage("Имя и пароль не должны быть пустыми");
        return false;
    }

    private void btnLogin_MouseMove(object? sender, MouseEventArgs e)
    {
        btnLogin.Cursor = Cursors.Hand;
    }

    private void btnRegister_MouseMove(object? sender, MouseEventArgs e)
    {
        btnRegister.Cursor = Cursors.Hand;
    }

    private void SetMode(LoginSwitch mode)
    {
        _mode = mode;

        var isLogin = mode == LoginSwitch.Login;

        lblLogin.Text = isLogin ? "Вход" : "Регистрация";
        lblLogin.Location = isLogin ? new Point(112, 30) : new Point(80, 30);

        btnLogin.Text = isLogin ? "Войти" : "Регистрация";

        lblAccount.Text = isLogin
            ? "У вас нет аккаунта?\nЗарегистрируйтесь!"
            : "У вас уже есть аккаунт?\nВойдите!";

        lblAccount.Location = isLogin
            ? new Point(90, 253)
            : new Point(80, 253);

        btnRegister.Text = isLogin ? "Регистрация" : "Вход";

        lblAccount.TextAlign = ContentAlignment.MiddleCenter;

        ClearInputs();
    }

    private void ClearInputs()
    {
        txtUsername.Clear();
        txtPassword.Clear();
    }

    #endregion
}

public enum LoginSwitch
{
    Login,
    Registration
}