using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Api.Controllers;

[Route("api/[controller]/[action]")]
public class AuthController(IAuthService service)
    : ControllerBase
{
    [HttpPost]
    public Task<IActionResult> Login([FromBody]LoginModel model)
    {
        return service.Login(model, HttpContext.RequestAborted);
    }
}