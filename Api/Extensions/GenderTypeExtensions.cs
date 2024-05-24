using Database.Enums;
using Models.Enums;

namespace Api.Extensions;

internal static class GenderTypeExtensions
{
    public static GenderType ConvertFromDbType(this DbGenderType dbGenderType)
    {
        return dbGenderType switch
        {
            DbGenderType.Male => GenderType.Male,
            DbGenderType.Female => GenderType.Female,
            _ => throw new ArgumentOutOfRangeException(
                nameof(dbGenderType), dbGenderType, null)
        };
    }
    
    public static DbGenderType ConvertToDbType(this GenderType dbGenderType)
    {
        return dbGenderType switch
        {
            GenderType.Male => DbGenderType.Male,
            GenderType.Female => DbGenderType.Female,
            _ => throw new ArgumentOutOfRangeException(
                nameof(dbGenderType), dbGenderType, null)
        };
    }
}