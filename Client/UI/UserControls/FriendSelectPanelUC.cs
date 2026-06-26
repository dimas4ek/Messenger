using Application.DTO;
using Client.Properties;
using Client.UI.Utils;
using Client.Utils;

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

        LanguageManager.LanguageChanged += ApplyLocalization;
        Disposed += (_, _) => LanguageManager.LanguageChanged -= ApplyLocalization;
        ApplyLocalization();

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
    
    #region Language
    
    private void ApplyLocalization()
    {
        addButton.Text = Strings.FriendSelectPanelUC_Add;
    }
    
    #endregion
}