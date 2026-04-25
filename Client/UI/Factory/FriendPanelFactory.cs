using Application.DTO;
using Client.Properties;
using Client.UI.Utils;
using Domain.Enums;
using Guna.UI2.WinForms;

namespace Client.UI.Factory;

public static class FriendPanelFactory
{
    public static Guna2Panel Create(
        UserInfo friend,
        MouseEventHandler onMove,
        EventHandler onLeave,
        MouseEventHandler onClick)
    {
        var friendPanel = new Guna2Panel();
        friendPanel.Size = new Size(210, 70);
        friendPanel.BackColor = Color.FromArgb(23, 33, 43);
        friendPanel.Dock = DockStyle.Top;
        friendPanel.Tag = friend;

        var avatar = new Guna2CirclePictureBox
        {
            Size = new Size(50, 50),
            Name = "avatarImage",
            Location = new Point(6, 10),
            SizeMode = PictureBoxSizeMode.StretchImage,
            Image = GetAvatar(friend),
            BackColor = Color.Transparent,
            Tag = friend
        };
        friendPanel.Controls.Add(avatar);

        var statusDot = new CirclePanel
        {
            Size = new Size(14, 14),
            Name = "statusDot",
            Location = new Point(avatar.Right - 14, avatar.Bottom - 14),
            BackColor = friend.Status == UserStatus.Online ? Color.Green : Color.Gray,
            Tag = friend
        };

        friendPanel.Controls.Add(statusDot);
        statusDot.BringToFront();

        var friendLabel = new Label();
        friendLabel.Name = "friendLabel";
        friendLabel.Parent = friendPanel;
        friendLabel.Text = friend.Username;
        friendLabel.Location = new Point(avatar.Size.Width + 10, 13);
        friendLabel.Font = new Font(FontFamily.GenericSansSerif, 12);
        friendLabel.BackColor = Color.FromArgb(23, 33, 43);
        friendLabel.ForeColor = Color.White;
        friendLabel.Tag = friend;

        friendPanel.MouseMove += onMove;
        friendPanel.MouseLeave += onLeave;
        friendPanel.MouseClick += onClick;

        PropagateMouseEvents(avatar, onMove, onLeave, onClick);
        PropagateMouseEvents(friendLabel, onMove, onLeave, onClick);

        return friendPanel;
    }

    public static void UpdateText(Guna2Panel panel, string newText)
    {
        if (panel.Controls.Find("friendLabel", false).FirstOrDefault() is Label label)
            label.Text = newText;
    }

    public static void UpdateAvatar(Guna2Panel panel, ImageInfo avatar)
    {
        if (panel.Controls.Find("avatarImage", false).FirstOrDefault() is Guna2CirclePictureBox pictureBox)
            pictureBox.Image = Image.FromStream(new MemoryStream(avatar.Data));
    }

    public static void UpdateStatus(Guna2Panel panel, UserStatus status)
    {
        if (panel.Controls.Find("statusDot", false).FirstOrDefault() is CirclePanel circlePanel)
            circlePanel.BackColor = status == UserStatus.Online ? Color.Green : Color.Gray;
    }

    private static Image GetAvatar(UserInfo friend)
    {
        if (friend.Avatar?.Data == null) return Resources.DefaultAvatarImage;

        using var ms = new MemoryStream(friend.Avatar.Data);
        return Image.FromStream(ms);
    }

    private static void PropagateMouseEvents(Control control, MouseEventHandler onMove, EventHandler onLeave,
        MouseEventHandler onClick)
    {
        control.MouseMove += onMove;
        control.MouseLeave += onLeave;
        control.MouseClick += onClick;
    }
}