using Database.Enums;
using Database.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Database;

public class EntityContext: BaseEntityContext
{
    protected string ConnectionString { get; set;}

    public EntityContext(IOptions<EntityContextOptions> options)
    {
        ConnectionString = options.Value.ConnectionString;
    }
        
    public EntityContext(string connectionString)
    {
        ConnectionString = connectionString;
    }
        
    public virtual DbSet<Department> Departments { get; set; }
    
    public virtual DbSet<Employee> Employees { get; set; }
    
    public virtual DbSet<Experience> Experiences { get; set; }
    
    public virtual DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
    
    public virtual DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Experience>().HasKey(i => new { i.ProgrammingLanguageID, i.EmployeeID });
        
        modelBuilder.Entity<Department>()
            .HasData(
                new List<Department>
                {
                    new(1, "Department 1", 1),
                    new(2, "Department 2", 2),
                    new(3, "Department 3", 3),
                });
        
        modelBuilder.Entity<ProgrammingLanguage>()
            .HasData(
                new List<ProgrammingLanguage>
                {
                    new (1, "C#"),
                    new (2, "JAVA"),
                });
        
        modelBuilder.Entity<User>()
            .HasData(
                new List<User>
                {
                    new (
                        1, 
                        "admin",
                        "A665A45920422F9D417E4867EFDC4FB8A04A1F3FFF1FA07E998E86F7F7A27AE3", // 123
                        DbUserRole.Admin),
                });
    }     
    /// <summary>
    /// 
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies(IsUseLazyLoading)
            .UseSqlServer(
                ConnectionString,
                builder =>
                {
                    //builder.EnableRetryOnFailure(30, TimeSpan.FromSeconds(2), null);
                });
    }
}