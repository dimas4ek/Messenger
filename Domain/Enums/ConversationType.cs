using NpgsqlTypes;

namespace Domain.Enums;

public enum ConversationType
{
    [PgName("private")] Private,
    [PgName("group")] Group
}