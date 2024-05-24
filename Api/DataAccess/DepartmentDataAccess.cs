using Database;
using Database.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;

namespace Api.DataAccess;

internal sealed class DepartmentDataAccess(IOptions<EntityContextOptions> options)
    : IDepartmentDataAccess
{
    public async Task<Result<IEnumerable<DepartmentModel>>> Get(CancellationToken cancellationToken)
    {
        try
        {
            await using var context = new EntityContext(options);
            var value = await context.Departments
                .OrderBy(i => i.Name)
                .Select(i => new DepartmentModel(i.ID, i.Name, i.Floor))
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return new Result<IEnumerable<DepartmentModel>>(value);
        }
        catch (Exception)
        {
            return new Result<IEnumerable<DepartmentModel>>(
                null,
                "Something error message",
                false);
        }
    }

    public async Task<Result<DepartmentModel>> Get(int id, CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);
        Department? department = null;
        try
        {
            department = await context.Departments
                .FirstOrDefaultAsync(i => i.ID == id, cancellationToken);
        }
        catch(Exception)
        {
            return new Result<DepartmentModel>(
                null,
                "Something error message",
                false);
        }

        if (department is null)
        {
            return new Result<DepartmentModel>(
                null,
                "Department not found",
                false);
        }

        return new Result<DepartmentModel>(
            new DepartmentModel(
                department.ID,
                department.Name,
                department.Floor));
    }
}