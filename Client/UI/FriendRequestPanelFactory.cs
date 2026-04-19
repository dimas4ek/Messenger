using Application.DTO;
using Guna.UI2.WinForms;

namespace Client.UI;

public static class FriendRequestPanelFactory
{
    public static Guna2Panel Create(
        FriendRequestInfo request,
        MouseEventHandler onMove,
        EventHandler onLeave,
        EventHandler onAccept,
        EventHandler onDecline)
    {
        var panel = new Guna2Panel();
        panel.Name = "requestPanel";
        panel.Size = new Size(360, 70);
        panel.BackColor = Color.FromArgb(23, 33, 43);
        panel.Dock = DockStyle.Top;
        panel.Tag = request;
        panel.MouseMove += onMove;
        panel.MouseLeave += onLeave;

        var label = new Label();
        label.Name = "requestLabel";
        label.Parent = panel;
        label.Text = request.Sender.Username;
        label.Location = new Point(6, 25);
        label.Font = new Font(FontFamily.GenericSansSerif, 12);
        label.BackColor = Color.FromArgb(23, 33, 43);
        label.ForeColor = Color.White;
        label.MouseMove += onMove;
        label.MouseLeave += onLeave;
        label.Tag = request;

        var acceptButton = new Guna2Button();
        acceptButton.Text = "✓";
        acceptButton.Size = new Size(40, 30);
        acceptButton.Location = new Point(270, 20);
        acceptButton.FillColor = Color.FromArgb(39, 174, 96);
        acceptButton.Tag = request;
        acceptButton.Click += onAccept;

        var declineButton = new Guna2Button();
        declineButton.Text = "✗";
        declineButton.Size = new Size(40, 30);
        declineButton.Location = new Point(315, 20);
        declineButton.FillColor = Color.FromArgb(192, 57, 43);
        declineButton.Tag = request;
        declineButton.Click += onDecline;

        panel.Controls.Add(label);
        panel.Controls.Add(acceptButton);
        panel.Controls.Add(declineButton);

        return panel;
    }
}