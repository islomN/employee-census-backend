using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Tables;

[Table("Experiences")]
[Index(nameof(EmployeeID), nameof(ProgrammingLanguageID), IsUnique = true)]
public record Experience(
    [property: Column("EmployeeID")]
    int EmployeeID,
    [property: Column("ProgrammingLanguageID")]
    int ProgrammingLanguageID)
{
    [ForeignKey("EmployeeID")]
    public Employee Employee { get; set; }

    [ForeignKey("ProgrammingLanguageID")]
    public ProgrammingLanguage ProgrammingLanguage { get; set; }
}