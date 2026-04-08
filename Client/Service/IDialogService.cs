namespace Client.Service;

public interface IDialogService
{
    void ShowMessage(string message);
    void ShowError(string message);
}