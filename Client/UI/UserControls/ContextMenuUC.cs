using Client.Properties;
using Client.Utils;

namespace Client.UI.UserControls;

public partial class ContextMenuUC : UserControl
{
    public ContextMenuUC(EventHandler onEditClick, EventHandler onDeleteClick, bool flag)
    {
        InitializeComponent();
        LanguageManager.LanguageChanged += ApplyLocalization;
        Disposed += (_, _) => LanguageManager.LanguageChanged -= ApplyLocalization;
        ApplyLocalization();

        editButton.Click += onEditClick;
        deleteButton.Click += onDeleteClick;

        editButton.Visible = flag;
        deleteButton.Location = flag ? new Point(5, 42) : new Point(5, 5);

        Size = flag ? new Size(160, 80) : new Size(160, 42);
    }
    
    #region Language
    
    private void ApplyLocalization()
    {
        editButton.Text = Strings.ContextMenuUC_Edit;
        deleteButton.Text = Strings.ContextMenuUC_Delete;
    }
    
    #endregion
}