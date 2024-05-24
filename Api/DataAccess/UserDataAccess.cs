using Api.Extensions;
using Api.Services;
using Database;
using Database.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;

namespace Api.DataAccess;

internal sealed class UserDataAccess(IOptions<EntityContextOptions> options)
    : IUserDataAccess
{
    public async Task<Result<IEnumerable<UserModel>>> Get(CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);

        try
        {
            var users = await context.Users.OrderBy(i => i.ID)
                .Select(i => new UserModel(i.ID, i.Username, null!, i.Role.ConvertFromDbType()))
                .ToListAsync(cancellationToken);

            return new Result<IEnumerable<UserModel>>(users);
        }
        catch (Exception)
        {
            return new Result<IEnumerable<UserModel>>(null, "Something error message");
        }
    }

    public async Task<Result<UserModel>> GetByUsername(string username, CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);

        try
        {
            var user = await context.Users
                .FirstOrDefaultAsync(i => i.Username == username, cancellationToken);

            if (user is null)
            {
                return new Result<UserModel>(null, "Not found", false);
            }

            return new Result<UserModel>(
                new UserModel(
                    user.ID,
                    user.Username,
                    user.PasswordHash,
                    user.Role.ConvertFromDbType()));
        }
        catch(Exception)
        {
            return new Result<UserModel>(
                null,
                "Something error message",
                false);
        }
    }

    public async Task<Result<EntityModel>> Add(UserRequestModel model, CancellationToken cancellationToken)
    {
        var user = new User(
            ID: default,
            Username: model.Username,
            PasswordHash: model.PasswordHash);

        await using var context = new EntityContext(options);

        try
        {
            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return new Result<EntityModel>(
                new EntityModel(user.ID));
        }
        catch (Exception)
        {
            return new Result<EntityModel>(
                null,
                "Something error message",
                false);
        }
    }

    public async Task<Result> UpdateActivity(int id, CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);
        User user;
        try
        {
            user = (await context.Users
                .FirstOrDefaultAsync(i => i.ID == id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new Result(
                "Something error message",
                false);
        }

        try
        {
            user!.LastActionDate = DateTime.Now;;

            await context.SaveChangesAsync();
            
            return new Result();
        }
        catch (Exception)
        {
            return new Result(
                "Something error message",
                false);
        }
    }
}