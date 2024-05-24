using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Api.Controllers;

[Route("api/[controller]/[action]")]
[Authorize]
public class EmployeeController(IEmployeeService service) : ControllerBase
{
    [HttpGet]
    public Task<IActionResult> Get()
    {
        return service.Get(HttpContext.RequestAborted);
    }

    [HttpGet("{id}")]
    public Task<IActionResult> Get(int id)
    {
        return service.Get(id, HttpContext.RequestAborted);
    }

    [HttpGet]
    public Task<IActionResult> GetNames(string key)
    {
        return service.GetNames(key, HttpContext.RequestAborted);
    }

    [HttpPost]
    public Task<IActionResult> Add([FromBody] EmployeeRequest request)
    {
        return service.Add(request, HttpContext.RequestAborted);
    }

    [HttpPut("{id}")]
    public Task<IActionResult> Edit([FromBody] EmployeeRequest request, int id)
    {
        return service.Edit(request, id, HttpContext.RequestAborted);
    }

    [HttpDelete]
    public Task<IActionResult> Delete(int id)
    {
        return service.Delete(id, HttpContext.RequestAborted);
    }
}