using System.Net;
using Api.ActionResults;
using Api.DataAccess;
using Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace Api.Services;

public class UserService(IUserDataAccess dataAccess)
    : IUserService
{
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await dataAccess.Get(cancellationToken);

        if (!result.IsSuccess)
        {
            return new ErrorActionResult(result.Message!);
        }

        return new SuccessActionResult(result.Payload!);
    }

    public async Task<IActionResult> Add(UserRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return new ErrorActionResult("Request model is empty", HttpStatusCode.BadRequest);
        }

        if (request.Username.IsNullOrEmpty() || request.Username.Length < 3)
        {
            return new ErrorActionResult("Username min len: 3", HttpStatusCode.BadRequest);
        }

        if (request.Password.IsNullOrEmpty() || request.Password.Length < 3)
        {
            return new ErrorActionResult("Password min len: 3", HttpStatusCode.BadRequest);
        }

        var userResult = await dataAccess
            .GetByUsername(request.Username, cancellationToken);

        if (userResult.Payload is not null)
        {
            return new ErrorActionResult("User already exists");
        }

        var result = await dataAccess.Add(
            new UserRequestModel(
                request.Username,
                CryptoHelper.ComputeSha256Hash(request.Password)),
            cancellationToken);

        if (result.IsSuccess)
        {
            return new SuccessActionResult();
        }

        return new ErrorActionResult(result.Message!);
    }
}