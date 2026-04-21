using Application.DTO;
using Client.Properties;
using Guna.UI2.WinForms;

namespace Client.UI;

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
        friendPanel.MouseMove += onMove;
        friendPanel.MouseLeave += onLeave;
        friendPanel.MouseClick += onClick;

        var avatar = new Guna2CirclePictureBox
        {
            Size = new Size(50, 50),
            Name = "avatarImage",
            Location = new Point(6, 10),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = GetAvatar(friend),
            BackColor = Color.Transparent
        };
        friendPanel.Controls.Add(avatar);

        var friendLabel = new Label();
        friendLabel.Name = "friendLabel";
        friendLabel.Parent = friendPanel;
        friendLabel.Text = friend.Username;
        friendLabel.Location = new Point(avatar.Size.Width + 10, 13);
        friendLabel.Font = new Font(FontFamily.GenericSansSerif, 12);
        friendLabel.BackColor = Color.FromArgb(23, 33, 43);
        friendLabel.ForeColor = Color.White;
        friendLabel.MouseMove += onMove;
        friendLabel.MouseLeave += onLeave;
        friendLabel.MouseClick += onClick;
        friendLabel.Tag = friend;

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

    private static Image GetAvatar(UserInfo friend)
    {
        if (friend.Avatar?.Data == null) return Resources.DefaultAvatar;

        using var ms = new MemoryStream(friend.Avatar.Data);
        return Image.FromStream(ms);
    }
}