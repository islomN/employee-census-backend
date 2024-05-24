using Api.Extensions;
using Api.Services;
using Database;
using Database.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;

namespace Api.DataAccess;

internal sealed class EmployeeDataAccess(IOptions<EntityContextOptions> options)
    : IEmployeeDataAccess
{
    public async Task<Result<IEnumerable<EmployeeModel>>> Get(
        CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);
        try
        {
            var value = await context.Employees
                .Where(i => !i.IsDeleted)
                .Include(i => i.Department)
                .Include(i => i.Experiences)
                .ThenInclude(i => i.ProgrammingLanguage)
                .OrderBy(i => i.ID)
                .Select(e => ConvertFromTableModel(e))
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return new Result<IEnumerable<EmployeeModel>>(value);
        }
        catch (Exception)
        {
            return new Result<IEnumerable<EmployeeModel>>(null,
                "Something error message",
                false);
        }
    }

    public async Task<Result<EmployeeModel>> Get(int id, CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);

        try
        {
            var employee = await context.Employees
                .Include(i => i.Department)
                .Include(i => i.Experiences)
                .ThenInclude(i => i.ProgrammingLanguage)
                .Where(i => i.ID == id && !i.IsDeleted)
                .Select(e => ConvertFromTableModel(e))
                .FirstOrDefaultAsync(cancellationToken);

            if (employee == null)
            {
                return new Result<EmployeeModel>(null, "Not found", false);
            }

            return new Result<EmployeeModel>(employee);
        }
        catch (Exception)
        {
            return new Result<EmployeeModel>(null, "Something error message");
        }
    }

    public async Task<Result<IEnumerable<string>>> GetNames(string key, CancellationToken cancellationToken)
    {
        try
        {
            key = key.ToLower();
            await using var context = new EntityContext(options);
            var value = await context.Employees
                .Where(i => i.FirstName.ToLower().StartsWith(key))
                .OrderBy(i => i.FirstName)
                .Select(i => i.FirstName)
                .Take(5)
                .Distinct()
                .ToListAsync(cancellationToken);

            return new Result<IEnumerable<string>>(value);
        }
        catch (Exception)
        {
            return new Result<IEnumerable<string>>(null,
                "Something error message",
                false);
        }
    }

    public async Task<Result<EntityModel>> Add(EmployeeRequestModel model, CancellationToken cancellationToken)
    {
        var employee = ConvertToTableModel(model);
        await using var context = new EntityContext(options);
        await using var transaction = await context.Database
            .BeginTransactionAsync(cancellationToken);

        try
        {
            await context.Employees.AddAsync(employee, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        
            var experiences = model.ProgrammingLanguageIDs
                .Select(i => 
                    new Experience(
                        EmployeeID: employee.ID,
                        ProgrammingLanguageID: i));
        
            await context.Experiences.AddRangeAsync(experiences, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            
            return new Result<EntityModel>(new EntityModel(employee.ID));
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return new Result<EntityModel>(
                null,
                "Something error message",
                false);
        }
    }

    public async Task<Result<EntityModel>> Edit(
        EmployeeRequestModel model,
        int id,
        CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);
        
        Employee? employee;
        
        try
        {
            employee = await context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.ID == id, cancellationToken);
        }
        catch (Exception)
        {
            return new Result<EntityModel>(
                null,
                "Something error message",
                false);
        }

        if (employee is null)
        {
            return new Result<EntityModel>(
                null,
                "Employee not found",
                false);
        }
        
        employee = employee with
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Age = model.Age,
            Gender = model.Gender,
            DepartmentID = model.DepartmentID,
        };
        
        var experiences = model.ProgrammingLanguageIDs
            .Select(i => 
                new Experience(
                    EmployeeID: employee.ID,
                    ProgrammingLanguageID: i));
        
        try
        {
            context.Experiences
                .RemoveRange(
                    context.Experiences
                        .Where(i => i.EmployeeID == employee.ID));
            
            await context.Experiences.AddRangeAsync(experiences, cancellationToken);
            context.Employees.Update(employee);
            await context.SaveChangesAsync(cancellationToken);

            return new Result<EntityModel>(
                new EntityModel(employee.ID));
        }
        catch (Exception)
        {
            return new Result<EntityModel>(
                null,
                "Something error message",
                false);
        }
    }

    public async Task<Result> Delete(int id, CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);
        Employee? employee;
        try
        {
            employee = await context.Employees
                .FirstOrDefaultAsync(i => i.ID == id, cancellationToken);
        }
        catch (Exception)
        {
            return new Result("Something error message", false);
        }
        
        if (employee is null)
        {
            return new Result("Employee not found", false);
        }

        employee = employee with
        {
            IsDeleted = true
        };
        
        try
        {
            //context.Employees.Edit(employee);

            await context.SaveChangesAsync(cancellationToken);
            return new Result();
        }
        catch(Exception)
        {
            return new Result("Something error message", false);
        }
    }

    private static Employee ConvertToTableModel(EmployeeRequestModel model)
    {
        return new Employee(
            ID: default,
            FirstName: model.FirstName,
            LastName: model.LastName,
            Age: model.Age,
            Gender: model.Gender,
            DepartmentID: model.DepartmentID,
            IsDeleted: false,
            CreatedDate: DateTime.Now,
            UpdatedDate: DateTime.Now);
    }

    private static EmployeeModel ConvertFromTableModel(Employee employee)
    {
        return new EmployeeModel(
            employee.ID,
            employee.FirstName,
            employee.LastName,
            employee.Age,
            employee.Gender.ConvertFromDbType(),
            employee.DepartmentID,
            employee.Department.Name,
            employee.Experiences
                .Select(i =>
                    new ProgrammingLanguageModel(
                        i.ProgrammingLanguage.ID,
                        i.ProgrammingLanguage.Name)));
    }
}