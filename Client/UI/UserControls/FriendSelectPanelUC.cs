using Application.DTO;
using Client.UI.Utils;

namespace Client.UI.UserControls;

public sealed partial class FriendSelectPanelUC : UserControl
{
    public FriendSelectPanelUC(
        UserInfo friend,
        MouseEventHandler onMove,
        EventHandler onLeave,
        EventHandler onAdd)
    {
        InitializeComponent();

        Tag = friend;
        Dock = DockStyle.Top;

        friendPanel.Tag = friend;

        friendLabel.Text = friend.Username;
        friendLabel.Tag = friend;

        addButton.Tag = friend;
        addButton.Click += onAdd;

        MouseEventUtils.PropagateMouseEvents(friendPanel, onMove, onLeave, null);
        MouseEventUtils.PropagateMouseEvents(friendLabel, onMove, onLeave, null);
    }
}