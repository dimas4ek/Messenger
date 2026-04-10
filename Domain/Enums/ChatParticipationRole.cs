using NpgsqlTypes;

namespace Domain.Enums;

public enum ChatParticipationRole
{
    [PgName("member")] Member,
    [PgName("admin")] Admin
}