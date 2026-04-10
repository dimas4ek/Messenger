using NpgsqlTypes;

namespace Domain.Enums;

public enum ChatType
{
    [PgName("private")] Private,
    [PgName("group")] Group
}