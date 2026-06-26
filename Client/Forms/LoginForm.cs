using Client.Controllers;
using Client.Forms.Base;
using Client.Properties;
using Client.Services;
using Client.UI.Utils;
using Client.Utils;

namespace Client.Forms;

public partial class LoginForm : BaseForm
{
    private readonly LoginAnimator _animator = null!;
    private readonly LoginController _controller = null!;
    private LoginSwitch _mode = LoginSwitch.Login;

    public LoginForm()
    {
    }

    public LoginForm(LoginController controller, IDialogService dialogService) : base(dialogService)
    {
        _controller = controller;
        _animator = new LoginAnimator(this);

        InitializeComponent();
        InitializeUI();
        LanguageManager.LanguageChanged += ApplyLocalization; 
        ApplyLocalization();
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
        Load += async (_, _) => await _animator.StartAsync();

        SetMode(LoginSwitch.Login);
        CenterPanel();
    }

    private void CenterPanel()
    {
        loginBackPanel.Left = (ClientSize.Width - loginBackPanel.Width) / 2;
        loginBackPanel.Top = (ClientSize.Height - loginBackPanel.Height) / 2;
    }

    #endregion

    #region Auth

    private void btnLogin_Click(object sender, EventArgs e)
    {
        _ = SafeInvoke(ProcessAuth);
    }

    private void txtPassword_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
            _ = SafeInvoke(ProcessAuth);
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

        if (!_controller.ValidateInput(username, password))
            return;

        ToggleControls(false, btnLogin, btnRegister);

        await SafeInvoke(async () =>
            {
                if (_mode == LoginSwitch.Login)
                    await HandleLogin(username, password);
                else
                    await HandleRegistration(username, password);
            },
            () => ToggleControls(true, btnLogin, btnRegister));
    }

    private async Task HandleLogin(string username, string password)
    {
        var login = await _controller.LoginAsync(username, password);
        if (!login) return;
        _controller.OpenClientForm();
        Hide();
    }

    private async Task HandleRegistration(string username, string password)
    {
        await _controller.RegisterAsync(username, password);
        SetMode(LoginSwitch.Login);
        ClearInputs();
    }

    #endregion

    #region Utils

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

        lblLogin.Text = isLogin ? Strings.AuthForm_Login : Strings.AuthForm_Register;
        lblLogin.Location = isLogin ? new Point(112, 30) : new Point(80, 30);

        btnLogin.Text = isLogin ? Strings.AuthForm_Login : Strings.AuthForm_Register;

        lblAccount.Text = isLogin ? Strings.AuthForm_NoAccount : Strings.AuthForm_HasAccount;
        lblAccount.Location = new Point(
            (Width - lblAccount.PreferredWidth) / 2, 253);
        
        btnRegister.Text = isLogin ? Strings.AuthForm_Register : Strings.AuthForm_Login;

        lblAccount.TextAlign = ContentAlignment.MiddleCenter;

        ClearInputs();
    }

    private void ClearInputs()
    {
        txtUsername.Clear();
        txtPassword.Clear();
    }

    #endregion
    
    #region Language
    
    private void ApplyLocalization()
    {
        label1.Text = Strings.AuthForm_Username;
        label2.Text = Strings.AuthForm_Password;
        CenterFieldLabels();
        SetMode(_mode);
    }

    private void CenterFieldLabels()
    {
        var centerX = txtUsername.Left + txtUsername.Width / 2;
        label1.Left = centerX - label1.PreferredWidth / 2;
        label2.Left = centerX - label2.PreferredWidth / 2;
    }

    private void ChangeLanguage(object sender, EventArgs e)
    {
        LanguageManager.SetLanguage(LanguageManager.CurrentLanguage == "ru" ? "en" : "ru");
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        LanguageManager.LanguageChanged -= ApplyLocalization;
        base.OnFormClosed(e);
    }
    
    #endregion
}

public enum LoginSwitch
{
    Login,
    Registration
}