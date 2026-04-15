using Client.Services;

namespace Client.Forms.Base;

public partial class BaseForm : Form
{
    protected IDialogService DialogService = null!;

    protected BaseForm()
    {
        InitializeComponent();
    }

    protected BaseForm(IDialogService dialogService)
    {
        DialogService = dialogService;
        InitializeComponent();
    }

    protected async Task SafeInvoke(Func<Task> action, Action? onFinally = null)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            DialogService.ShowError(ex.Message);
        }
        finally
        {
            onFinally?.Invoke();
        }
    }

    protected void ToggleControls(bool enabled, params Control[] controls)
    {
        foreach (var c in controls) c.Enabled = enabled;
    }
}