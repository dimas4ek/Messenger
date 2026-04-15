namespace Client.Services;

public interface IDialogService
{
    void ShowMessage(string message);
    void ShowError(string message);
}