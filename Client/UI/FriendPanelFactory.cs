using Application.DTO;
using Guna.UI2.WinForms;

namespace Client.UI;

public class FriendPanelFactory
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
        friendLabel.Tag = friend;

        return friendPanel;
    }

    public static void SetColor(Guna2Panel panel, int r, int g, int b)
    {
        panel.BackColor = Color.FromArgb(r, g, b);

        var label = panel.Controls.OfType<Label>().FirstOrDefault();
        if (label == null) return;

        label.BackColor = Color.FromArgb(r, g, b);
        label.ForeColor = Color.White;
    }
}