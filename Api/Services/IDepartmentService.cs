using Microsoft.AspNetCore.Mvc;

namespace Api.Services;

public interface IDepartmentService
{
    Task<IActionResult> Get(CancellationToken cancellationToken);
}