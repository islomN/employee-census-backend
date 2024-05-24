using Database.Enums;
using Models.Enums;

namespace Api.Extensions;

internal static class UserRoleExtensions
{
    public static Role ConvertFromDbType(this DbUserRole dbGenderType)
    {
        return dbGenderType switch
        {
            DbUserRole.General => Role.General,
            DbUserRole.Admin => Role.Admin,
            _ => throw new ArgumentOutOfRangeException(
                nameof(dbGenderType), dbGenderType, null)
        };
    }
}