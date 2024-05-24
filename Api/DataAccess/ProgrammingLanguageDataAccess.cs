using Database;
using Database.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;

namespace Api.DataAccess;

internal sealed class ProgrammingLanguageDataAccess(IOptions<EntityContextOptions> options)
    : IProgrammingLanguageDataAccess
{
    public async Task<Result<IEnumerable<ProgrammingLanguageModel>>> Get(CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);
        try
        {
            var value = await context.ProgrammingLanguages
                .OrderBy(i => i.Name)
                .Select(i => new ProgrammingLanguageModel(
                    i.ID,
                    i.Name))
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return new Result<IEnumerable<ProgrammingLanguageModel>>(value);
        }
        catch (Exception)
        {
            return new Result<IEnumerable<ProgrammingLanguageModel>>(
                null,
                "Something error message",
                false);
        }
    }

    public async Task<Result<ProgrammingLanguageModel>> Get(int id, CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);
        ProgrammingLanguage? programmingLanguage = null;
        try
        {
            programmingLanguage = await context.ProgrammingLanguages
                .FirstOrDefaultAsync(i => i.ID == id, cancellationToken);
        }
        catch(Exception)
        {
            return new Result<ProgrammingLanguageModel>(
                null,
                "Something error message",
                false);
        }

        if (programmingLanguage is null)
        {
            return new Result<ProgrammingLanguageModel>(
                null,
                "Department not found",
                false);
        }

        return new Result<ProgrammingLanguageModel>(
            new ProgrammingLanguageModel(
                programmingLanguage.ID,
                programmingLanguage.Name));
    }

    public async Task<Result<bool>> Any(IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);

        try
        {
            var programmingLanguages = await context.ProgrammingLanguages
                .Select(i => i.ID)
                .ToListAsync(cancellationToken);
            var payload = ids.All(i => programmingLanguages.Contains(i));

            return new Result<bool>(payload);
        }
        catch (Exception)
        {
            return new Result<bool>(default, "Something error message", false);
        }
    }
}