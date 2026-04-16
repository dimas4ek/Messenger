using Client.Api.Clients;
using Client.Forms;
using Client.Services;
using Client.Utils;
using Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Controllers;

public class LoginController(
    IServiceProvider serviceProvider,
    IDialogService dialogService,
    AuthApiClient authApiClient,
    UserContext userContext)
{
    public bool ValidateInput(string username, string password)
    {
        if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password)) return true;
        dialogService.ShowMessage("Имя и пароль не должны быть пустыми");
        return false;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var result = await authApiClient.Login(username, password);

        if (!result.IsSuccess || result.Value == null)
        {
            dialogService.ShowError(result.ToMessage());
            return false;
        }

        //todo terminate if logged in already

        var user = result.Value.User;

        if (user == null)
        {
            dialogService.ShowError("Пользователь не найден");
            return false;
        }

        user.Status = UserStatus.Online;

        userContext.SetUser(user);

        return true;
    }

    public async Task<bool> RegisterAsync(string username, string password)
    {
        var result = await authApiClient.Register(username, password);

        if (!result.IsSuccess)
        {
            dialogService.ShowError(result.ToMessage());
            return false;
        }

        dialogService.ShowMessage("Регистрация успешна! Теперь можете войти.");

        return true;
    }

    public void OpenClientForm()
    {
        var client = serviceProvider.GetRequiredService<ClientForm>();
        client.Show();
    }
}