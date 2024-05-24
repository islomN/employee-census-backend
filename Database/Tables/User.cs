using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Database.Enums;
using Microsoft.EntityFrameworkCore;

namespace Database.Tables;

[Table("Users")]
[Index(nameof(Username), IsUnique = true)]
public record User(
    [property: Column("ID")]
    int ID,
    [property: Column("Username")]
    [property: Required]
    [property: MinLength(3)]
    string Username,
    [property: Column("PasswordHash")]
    [property: Required]
    string PasswordHash,
    [property: Column("Role")] DbUserRole Role = DbUserRole.General)
{
    [Column("LastActionDate")]
    public DateTime? LastActionDate { get; set; }
    
    [Column("CreatedDate")]
    public DateTime CreatedDate { get; set; } = DateTime.Now;
};