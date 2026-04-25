using Application.DTO;
using Guna.UI2.WinForms;

namespace Client.UI.Factory;

public static class FriendSelectPanelFactory
{
    public static Guna2Panel Create(
        UserInfo friend,
        MouseEventHandler onMove,
        EventHandler onLeave,
        EventHandler onAdd)
    {
        var panel = new Guna2Panel();
        panel.Size = new Size(330, 60);
        panel.BackColor = Color.FromArgb(23, 33, 43);
        panel.Dock = DockStyle.Top;
        panel.Tag = friend;
        panel.MouseMove += onMove;
        panel.MouseLeave += onLeave;

        var label = new Label();
        label.Text = friend.Username;
        label.Location = new Point(6, 18);
        label.Font = new Font(FontFamily.GenericSansSerif, 12);
        label.BackColor = Color.FromArgb(23, 33, 43);
        label.ForeColor = Color.White;
        label.AutoSize = true;
        label.MouseMove += onMove;
        label.MouseLeave += onLeave;
        label.Tag = friend;

        var addButton = new Guna2Button();
        addButton.Text = "Add";
        addButton.Size = new Size(70, 30);
        addButton.Location = new Point(260, 15);
        addButton.FillColor = Color.FromArgb(114, 137, 218);
        addButton.ForeColor = Color.White;
        addButton.Tag = friend;
        addButton.Click += onAdd;

        panel.Controls.Add(label);
        panel.Controls.Add(addButton);

        return panel;
    }
}