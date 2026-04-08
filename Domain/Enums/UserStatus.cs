using NpgsqlTypes;

namespace Domain.Enums;

public enum UserStatus
{
    [PgName("offline")] Offline,
    [PgName("online")] Online
}