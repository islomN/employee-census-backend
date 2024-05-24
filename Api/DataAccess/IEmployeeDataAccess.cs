using Api.Services;
using Models;

namespace Api.DataAccess;

public interface IEmployeeDataAccess
{
    Task<Result<IEnumerable<EmployeeModel>>> Get(CancellationToken cancellationToken);
    
    Task<Result<EmployeeModel>> Get(int id, CancellationToken cancellationToken);

    Task<Result<IEnumerable<string>>> GetNames(string key, CancellationToken cancellationToken);

    Task<Result<EntityModel>> Add(EmployeeRequestModel model, CancellationToken cancellationToken);

    Task<Result<EntityModel>> Edit(EmployeeRequestModel model, int id, CancellationToken cancellationToken);

    Task<Result> Delete(int id, CancellationToken cancellationToken);
}