using System.Net;
using Api.ActionResults;
using Api.DataAccess;
using Api.Helpers;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace Api.Services;

internal sealed class AuthService(IUserDataAccess dataAccess)
    : IAuthService
{
    public async Task<IActionResult> Login(LoginModel model, CancellationToken cancellationToken)
    {
        if (!IsValid(model, out var errorMessage))
        {
            return new ErrorActionResult(errorMessage!, HttpStatusCode.BadRequest);
        }

        var result = await dataAccess.GetByUsername(model.Username, cancellationToken);

        if (!result.IsSuccess)
        {
            return new ErrorActionResult(result.Message!);
        }

        if (result.Payload is null)
        {
            return new ErrorActionResult("User not found", HttpStatusCode.BadRequest);
        }

        if (result.Payload.PasswordHash != CryptoHelper.ComputeSha256Hash(model.Password))
        {
            return new ErrorActionResult("Password not valid", HttpStatusCode.BadRequest);
        }

        return new SuccessActionResult(result.Payload with
        {
            PasswordHash = null!,
        });
    }

    private static bool IsValid(LoginModel model, out string? errorMessage)
    {
        if (model is null)
        {
            errorMessage = "Model is empty";
            return false;
        }

        if (model.Username.IsNullOrEmpty())
        {
            errorMessage = "Username is empty";
            return false;
        }
        
        if (model.Password.IsNullOrEmpty())
        {
            errorMessage = "Username is empty";
            return false;
        }

        errorMessage = null;
        
        return true;
    }
}