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
    InvalidUsername,
    MessageNotFound,
    DatabaseError,

    // Common
    ValidationError,
    UnknownError
}