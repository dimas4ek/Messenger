using Client.Properties;

namespace Client.Services;

public class DialogService : IDialogService
{
    public void ShowMessage(string message)
    {
        MessageBox.Show(message, Strings.DialogService_Info, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public void ShowError(string message)
    {
        MessageBox.Show(message, Strings.DialogService_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}