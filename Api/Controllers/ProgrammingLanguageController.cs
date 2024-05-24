using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]/[action]")]
[Authorize]
public class ProgrammingLanguageController(IProgrammingLanguageService service)
    : ControllerBase
{
    [HttpGet]
    public Task<IActionResult> Get()
    {
        return service.Get(HttpContext.RequestAborted);
    }
}