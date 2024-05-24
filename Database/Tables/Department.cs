using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Tables;

[Table("Departments")]
public record Department(
    [property: Column("ID")]
    int ID,
    [property: Column("Name")]
    [property: Required]
    [property: MinLength(1)]
    string Name,
    [property: Column("Floor")]
    [property: Required]
    short Floor);