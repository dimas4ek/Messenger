using Guna.UI2.WinForms;

namespace Client.UI.Utils;

public static class MouseEventUtils
{
    public static void PropagateMouseEvents(Control control, MouseEventHandler onMove, EventHandler onLeave,
        MouseEventHandler? onClick)
    {
        control.MouseMove += onMove;
        control.MouseLeave += onLeave;
        control.MouseClick += onClick;
    }

    public static void OnFriendPanelMove(object? s, Action<Guna2Panel>? onPanel = null)
    {
        if (s is not Control c) return;
        c.Cursor = Cursors.Hand;
        var panel = c as Guna2Panel ?? c.Parent as Guna2Panel;
        if (panel != null) onPanel?.Invoke(panel);
    }
}