using Models.Enums;

namespace Models;

public record EmployeeRequest(
    string FirstName,
    string LastName,
    byte Age,
    GenderType Gender,
    int DepartmentID,
    IEnumerable<int> ProgrammingLanguageIDs
);