using Models;

namespace Api.DataAccess;

public interface IDepartmentDataAccess
{
    Task<Result<IEnumerable<DepartmentModel>>> Get(CancellationToken cancellationToken);

    Task<Result<DepartmentModel>> Get(int id, CancellationToken cancellationToken);
}