using Microsoft.AspNetCore.Mvc;

namespace Api.Services;

public interface IProgrammingLanguageService
{
    Task<IActionResult> Get(CancellationToken cancellationToken);
}