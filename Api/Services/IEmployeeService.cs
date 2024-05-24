using Microsoft.AspNetCore.Mvc;
using Models;

namespace Api.Services;

public interface IEmployeeService
{
    Task<IActionResult> Get(CancellationToken cancellationToken);
    
    Task<IActionResult> Get(int id, CancellationToken cancellationToken);

    Task<IActionResult> GetNames(string key, CancellationToken cancellationToken);

    Task<IActionResult> Add(EmployeeRequest request, CancellationToken cancellationToken);

    Task<IActionResult> Edit(EmployeeRequest request, int id, CancellationToken cancellationToken);

    Task<IActionResult> Delete(int id, CancellationToken cancellationToken);
}