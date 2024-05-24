using Api.Services;
using Models;

namespace Api.DataAccess;

public interface IUserDataAccess
{
    Task<Result<IEnumerable<UserModel>>> Get(CancellationToken cancellationToken);
    
    Task<Result<UserModel>> GetByUsername(string username, CancellationToken cancellationToken);

    Task<Result<EntityModel>> Add(UserRequestModel model, CancellationToken cancellationToken);

    Task<Result> UpdateActivity(int id, CancellationToken cancellationToken);
}