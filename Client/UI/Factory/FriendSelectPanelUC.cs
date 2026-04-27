using Application.DTO;

namespace Client.UI.Factory;

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
        friendPanel.MouseMove += onMove;
        friendPanel.MouseLeave += onLeave;

        friendLabel.Text = friend.Username;
        friendLabel.MouseMove += onMove;
        friendLabel.MouseLeave += onLeave;
        friendLabel.Tag = friend;

        addButton.Tag = friend;
        addButton.Click += onAdd;
    }
}