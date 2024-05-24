using Microsoft.AspNetCore.Mvc;
using Models;

namespace Api.Services;

public interface IUserService
{
    Task<IActionResult> Get(CancellationToken cancellationToken);
    
    Task<IActionResult> Add(UserRequest request, CancellationToken cancellationToken);
}