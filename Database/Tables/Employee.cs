using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Database.Enums;

namespace Database.Tables;

[Table("Employees")]
public record Employee(
    [property: Column("ID")]
    int ID,
    [property: Column("FirstName")]
    [property: Required]
    string FirstName,
    [property: Column("LastName")]
    [property: MinLength(1)]
    string LastName,
    [property: Column("Age")]
    [property: Required]
    byte Age,
    [property: Column("Gender")]
    [property: Required]
    DbGenderType Gender,
    [property: Column("DepartmentID")]
    int DepartmentID,
    [property: Column("IsDeleted")]
    bool IsDeleted,
    [property: Column("CreatedDate")]
    DateTime CreatedDate,
    [property: Column("UpdatedDate")]
    DateTime UpdatedDate)
{
    [ForeignKey("DepartmentID")]
    public Department Department { get; set; }
    
    public ICollection<Experience> Experiences { get; set; }
}