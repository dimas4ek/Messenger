namespace Application.Utils;

public enum ErrorCode
{
    None = 0,

    // Auth
    InvalidPassword,
    UsernameTaken,
    NotAuthenticated,
    EmptyResponse,
    AuthError,

    // Database
    UserNotFound,
    ChatNotFound,
    FriendRequestNotFound,
    InvalidUsername,
    MessageNotFound,
    DatabaseError,
    AccessDenied,

    // Common
    ValidationError,
    UnknownError
}