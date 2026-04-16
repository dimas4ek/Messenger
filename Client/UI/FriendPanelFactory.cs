using Application.DTO;
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

        var friendLabel = new Label();
        friendLabel.Parent = friendPanel;
        friendLabel.Text = friend.Username;
        friendLabel.Location = new Point(6, 13);
        friendLabel.Font = new Font(FontFamily.GenericSansSerif, 12);
        friendLabel.BackColor = Color.FromArgb(23, 33, 43);
        friendLabel.ForeColor = Color.White;
        friendLabel.MouseMove += onMove;
        friendLabel.MouseLeave += onLeave;
        friendLabel.MouseClick += onClick;
        friendLabel.Tag = friend;

        return friendPanel;
    }
}