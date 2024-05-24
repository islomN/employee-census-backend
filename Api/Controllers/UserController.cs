using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Api.Controllers;

[Route("api/[controller]/[action]")]
[Authorize(Roles = "Admin")]
public class UserController(IUserService service)
    : ControllerBase
{
    [HttpGet]
    public Task<IActionResult> Get()
    {
        return service.Get(HttpContext.RequestAborted);
    }
    
    [HttpPost]
    public Task<IActionResult> Add([FromBody]UserRequest request)
    {
        return service.Add(request, HttpContext.RequestAborted);
    }
}