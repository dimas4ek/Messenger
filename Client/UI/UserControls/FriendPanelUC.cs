using Application.DTO;
using Client.UI.Utils;
using Client.Utils;

namespace Client.UI.UserControls;

public sealed partial class FriendPanelUC : UserControl
{
    public FriendPanelUC(
        UserInfo friend,
        MouseEventHandler onMove,
        EventHandler onLeave
    )
    {
        InitializeComponent();

        Tag = friend;
        Dock = DockStyle.Top;

        friendPanel.Tag = friend;

        friendAvatar.Image = ImageHelper.GetAvatar(friend.Avatar);

        friendLabel.Text = friend.Username;
        friendLabel.Tag = friend;

        MouseEventUtils.PropagateMouseEvents(friendPanel, onMove, onLeave, null);
        MouseEventUtils.PropagateMouseEvents(friendAvatar, onMove, onLeave, null);
        MouseEventUtils.PropagateMouseEvents(friendLabel, onMove, onLeave, null);
    }
}