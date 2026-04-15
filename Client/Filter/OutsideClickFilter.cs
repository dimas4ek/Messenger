namespace Client.Filter;

public class OutsideClickFilter(Control menu, Control form, Action onClose) : IMessageFilter
{
    private readonly Control _form = form;

    private const int WM_LBUTTONDOWN = 0x0201;
    private const int WM_RBUTTONDOWN = 0x0204;

    public bool PreFilterMessage(ref Message m)
    {
        if (m.Msg != WM_LBUTTONDOWN && m.Msg != WM_RBUTTONDOWN)
            return false;

        var cursorPos = Cursor.Position;
        var menuBounds = menu.RectangleToScreen(new Rectangle(Point.Empty, menu.Size));

        if (menuBounds.Contains(cursorPos)) return false;
        onClose();
        return false;

    }
}