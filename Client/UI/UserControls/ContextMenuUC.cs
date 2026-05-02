namespace Client.UI.UserControls;

public partial class ContextMenuUC : UserControl
{
    public ContextMenuUC(EventHandler onEditClick, EventHandler onDeleteClick, bool flag)
    {
        InitializeComponent();

        editButton.Click += onEditClick;
        deleteButton.Click += onDeleteClick;

        editButton.Visible = flag;
        deleteButton.Location = flag ? new Point(5, 42) : new Point(5, 5);

        Size = flag ? new Size(160, 80) : new Size(160, 42);
    }
}