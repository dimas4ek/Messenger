namespace Application.Utils;

public static class ErrorMapper
{
    public static string ToMessage(ErrorCode code)
    {
        return code switch
        {
            ErrorCode.UserNotFound => "Пользователь не найден",
            ErrorCode.ChatNotFound => "Чат не найден",
            ErrorCode.InvalidPassword => "Неверный пароль",
            ErrorCode.UsernameTaken => "Имя уже занято",
            ErrorCode.NotAuthenticated => "Пользователь не аутентифицирован",
            ErrorCode.InvalidUsername => "Неверное имя пользователя",
            ErrorCode.MessageNotFound => "Сообщение не найдено",
            ErrorCode.DatabaseError => "Ошибка базы данных",
            ErrorCode.EmptyResponse => "Сервер вернул пустой ответ",
            ErrorCode.AuthError => "Ошибка авторизации",
            ErrorCode.AccessDenied => "Нет доступа",

            ErrorCode.ValidationError => "Ошибка ввода",
            _ => "Неизвестная ошибка"
        };
    }
}