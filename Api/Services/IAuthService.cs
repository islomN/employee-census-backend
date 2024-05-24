using Microsoft.AspNetCore.Mvc;
using Models;

namespace Api.Services;

public interface IAuthService
{
    Task<IActionResult> Login(LoginModel model, CancellationToken cancellationToken);
}