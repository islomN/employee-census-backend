using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Tables;

[Table("ProgrammingLanguages")]
[Index(nameof(Name), IsUnique = true)]
public record ProgrammingLanguage(
    [property: Column("ID")]
    int ID,
    [property: Column("Name")]
    [property: Required]
    string Name);