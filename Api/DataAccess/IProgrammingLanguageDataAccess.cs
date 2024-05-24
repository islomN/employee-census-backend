using Models;

namespace Api.DataAccess;

public interface IProgrammingLanguageDataAccess
{
    Task<Result<IEnumerable<ProgrammingLanguageModel>>> Get(CancellationToken cancellationToken);

    Task<Result<ProgrammingLanguageModel>> Get(int id, CancellationToken cancellationToken);

    Task<Result<bool>> Any(IEnumerable<int> ids, CancellationToken cancellationToken);
}