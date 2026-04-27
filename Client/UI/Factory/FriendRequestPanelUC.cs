using Application.DTO;

namespace Client.UI.Factory;

public sealed partial class FriendRequestPanelUC : UserControl
{
    public FriendRequestPanelUC(
        FriendRequestInfo request,
        MouseEventHandler onMove,
        EventHandler onLeave,
        EventHandler onAccept,
        EventHandler onDecline)
    {
        InitializeComponent();

        Dock = DockStyle.Top;
        Tag = request;
        MouseMove += onMove;
        MouseLeave += onLeave;

        requestPanel.Tag = request;
        requestPanel.MouseMove += onMove;
        requestPanel.MouseLeave += onLeave;

        requestLabel.Text = request.Sender.Username;
        requestLabel.MouseMove += onMove;
        requestLabel.MouseLeave += onLeave;
        requestLabel.Tag = request;

        acceptButton.Text = "✓";
        acceptButton.Tag = request;
        acceptButton.Click += onAccept;

        declineButton.Text = "✗";
        declineButton.Tag = request;
        declineButton.Click += onDecline;
    }
}