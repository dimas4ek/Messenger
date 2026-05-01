using Application.DTO;
using Client.UI.Utils;

namespace Client.UI.UserControls;

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

        requestPanel.Tag = request;

        requestLabel.Text = request.Sender.Username;
        requestLabel.Tag = request;

        acceptButton.Text = "✓";
        acceptButton.Tag = request;
        acceptButton.Click += onAccept;

        declineButton.Text = "✗";
        declineButton.Tag = request;
        declineButton.Click += onDecline;

        MouseEventUtils.PropagateMouseEvents(this, onMove, onLeave, null);
        MouseEventUtils.PropagateMouseEvents(requestPanel, onMove, onLeave, null);
        MouseEventUtils.PropagateMouseEvents(requestLabel, onMove, onLeave, null);
    }
}