using NpgsqlTypes;

namespace Domain.Enums;

public enum ParticipationRole
{
    [PgName("member")] Member,
    [PgName("admin")] Admin
}