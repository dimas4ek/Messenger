namespace Application.DTO;

public class UserContext
{
    public UserInfo? CurrentUser { get; private set; }

    public bool IsLoggedIn => CurrentUser != null;

    public void SetUser(UserInfo user)
    {
        CurrentUser = user;
    }

    public void Clear()
    {
        CurrentUser = null;
    }
}