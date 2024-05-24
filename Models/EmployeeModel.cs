using Models.Enums;

namespace Models;

public record EmployeeModel(
    int ID,
    string FirstName,
    string LastName,
    byte Age,
    GenderType Gender,
    int DepartmentID,
    string Department,
    IEnumerable<ProgrammingLanguageModel> Experiences
);