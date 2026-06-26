using Application.DTO;
using Client.Forms.Dialogs;
using Client.Properties;
using Client.Utils;

namespace Client.UI.UserControls;

public sealed partial class ProfilePanelUC : UserControl
{
    public ProfilePanelUC(
        UserInfo currentUser, int width, int height, Action onClose,
        KeyEventHandler onFriendKeyDown,
        Action<UserInfo> onUserUpdated)
    {
        InitializeComponent();
        LanguageManager.LanguageChanged += ApplyLocalization;
        Disposed += (_, _) => LanguageManager.LanguageChanged -= ApplyLocalization;
        ApplyLocalization();

        Size = new Size(width, height);
        Location = new Point(0, 0);
        Dock = DockStyle.Left;
        Visible = false;

        usernameLabel.Text = currentUser.Username;
        usernameLabel.Location = new Point(
            (width - usernameLabel.PreferredWidth) / 2, 20);

        closeProfileButton.CheckedState.ImageSize = new Size(20, 20);
        closeProfileButton.HoverState.ImageSize = new Size(20, 20);
        closeProfileButton.PressedState.ImageSize = new Size(20, 20);
        closeProfileButton.Click += (_, _) => onClose();

        addFriendButton.Size = new Size(width, 45);
        addFriendButton.Click += (_, _) => AddFriendButton_Click(onFriendKeyDown);

        settingsButton.Click += (_, _) =>
        {
            var dialog = new ProfileSettingsDialog(currentUser, onUserUpdated);
            dialog.ShowDialog();
        };
    }

    public string FriendTextBoxText => addFriendTextBox.Text;

    private void AddFriendButton_Click(KeyEventHandler onFriendKeyDown)
    {
        if (!addFriendTextBox.Visible)
        {
            addFriendTextBox.Visible = true;
            addFriendTextBox.Size = new Size(profilePanel.Width, 45);
            addFriendTextBox.KeyDown += onFriendKeyDown;
            return;
        }

        addFriendTextBox.Visible = false;
    }

    public void UpdateUsername(string username)
    {
        usernameLabel.Text = username;
    }

    public void ClearFriendTextBox()
    {
        addFriendTextBox.Clear();
    }

    #region Language

    private void ApplyLocalization()
    {
        addFriendButton.Text = Strings.ProfilePanelUC_AddFriend;
        addFriendTextBox.PlaceholderText = Strings.ProfilePanelUC_EnterFriendName;
        settingsButton.Text = Strings.ProfilePanelUC_Settings;
    }

    #endregion
}