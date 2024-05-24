using Database.Enums;

namespace Models;

public record EmployeeRequestModel(
    string FirstName,
    string LastName,
    byte Age,
    DbGenderType Gender,
    int DepartmentID,
    IEnumerable<int> ProgrammingLanguageIDs);