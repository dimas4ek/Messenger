namespace Client.Service;

public class DialogService : IDialogService
{
    public void ShowMessage(string message)
    {
        MessageBox.Show(message, @"Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public void ShowError(string message)
    {
        MessageBox.Show(message, @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}