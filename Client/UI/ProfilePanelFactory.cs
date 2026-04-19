using Application.DTO;
using Client.Forms.Dialogs;
using Client.Properties;
using Guna.UI2.WinForms;

namespace Client.UI;

public static class ProfilePanelFactory
{
    public static Guna2Panel Create(UserInfo currentUser, int width, int height, Action onClose,
        EventHandler onAddFriend,
        Action<UserInfo> onUserUpdated, Action<UserInfo> onFriendAdded)
    {
        var profilePanel = new Guna2Panel
        {
            Size = new Size(width, height),
            BackColor = Color.FromArgb(23, 33, 43),
            Location = new Point(0, 0),
            Dock = DockStyle.Left,
            Visible = false
        };

        var profileUsernameLabel = new Label
        {
            Parent = profilePanel,
            Name = "usernameLabel",
            Font = new Font(FontFamily.GenericSansSerif, 15.75f),
            ForeColor = Color.White,
            Text = currentUser.Username,
            AutoSize = true
        };
        profileUsernameLabel.Location = new Point(
            (width - profileUsernameLabel.PreferredWidth) / 2, 20);

        var btnCloseProfile = new Guna2ImageButton
        {
            Parent = profilePanel,
            ImageSize = new Size(20, 20),
            Image = (Image)Resources.ResourceManager.GetObject("btnProfileImage")!,
            ImageOffset = new Point(0, 0),
            ImageRotate = 0F,
            Location = new Point(23, 17),
            Margin = new Padding(4, 3, 4, 3),
            Name = "btnOpenProfile",
            Size = new Size(40, 40),
            Cursor = Cursors.Hand,
            Anchor = AnchorStyles.Top | AnchorStyles.Left
        };
        btnCloseProfile.CheckedState.ImageSize = new Size(20, 20);
        btnCloseProfile.HoverState.ImageSize = new Size(20, 20);
        btnCloseProfile.PressedState.ImageSize = new Size(20, 20);
        btnCloseProfile.Click += (_, _) => onClose();

        var addFriendButton = new Guna2Button
        {
            Parent = profilePanel,
            Location = new Point(0, 58),
            Size = new Size(width, 45),
            FillColor = Color.FromArgb(23, 33, 43),
            HoverState = { FillColor = Color.FromArgb(35, 46, 60) },
            Text = "Add Friend",
            Font = new Font("Segoe UI", 12),
            TextAlign = HorizontalAlignment.Center
        };

        addFriendButton.Click += onAddFriend;

        profilePanel.Controls.Add(FriendRequestsButton(width, currentUser, onFriendAdded));
        profilePanel.Controls.Add(SettingsButton(width, currentUser, onUserUpdated));

        return profilePanel;
    }

    private static Guna2Button FriendRequestsButton(int width, UserInfo currentUser, Action<UserInfo> onFriendAdded)
    {
        var friendRequestsButton = new Guna2Button
        {
            Size = new Size(width, 50),
            FillColor = Color.FromArgb(23, 33, 43),
            HoverState = { FillColor = Color.FromArgb(35, 46, 60) },
            Dock = DockStyle.Bottom,
            Visible = true,
            Text = "Friend Requests",
            Font = new Font(FontFamily.GenericSansSerif, 15.75f),
            ForeColor = Color.White,
            TextAlign = HorizontalAlignment.Center,
            Cursor = Cursors.Hand
        };
        friendRequestsButton.Click += (_, _) =>
        {
            var dialog = new FriendRequestsDialog(currentUser, onFriendAdded);
            dialog.ShowDialog();
        };

        return friendRequestsButton;
    }

    private static Guna2Button SettingsButton(int width, UserInfo currentUser, Action<UserInfo> onUserUpdated)
    {
        var settingsButton = new Guna2Button
        {
            Size = new Size(width, 50),
            FillColor = Color.FromArgb(23, 33, 43),
            HoverState = { FillColor = Color.FromArgb(35, 46, 60) },
            Dock = DockStyle.Bottom,
            Visible = true,
            Text = "Settings",
            Font = new Font(FontFamily.GenericSansSerif, 15.75f),
            ForeColor = Color.White,
            TextAlign = HorizontalAlignment.Center,
            Cursor = Cursors.Hand
        };
        settingsButton.Click += (_, _) =>
        {
            var dialog = new ProfileSettingsDialog(currentUser, onUserUpdated);
            dialog.ShowDialog();
        };

        return settingsButton;
    }
}