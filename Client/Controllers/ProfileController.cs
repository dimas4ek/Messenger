using Client.Api.Clients;
using Client.Services;
using Client.Utils;

namespace Client.Controllers;

public class ProfileController(AuthApiClient authApiClient, IDialogService dialogService, UserContext userContext)
{
    public async Task LogoutAsync()
    {
        var currentUser = userContext.CurrentUser;
        if (currentUser == null) return;

        var result = await authApiClient.Logout(currentUser.Id);
        if (!result.IsSuccess || result.Value == null)
            dialogService.ShowError(result.ToMessage());

        userContext.Clear();
    }
}