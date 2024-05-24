using System.Net;
using Api.ActionResults;
using Api.DataAccess;
using Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace Api.Services;

internal sealed class EmployeeService(
        IEmployeeDataAccess employeeDataAccess,
        IDepartmentDataAccess departmentDataAccess,
        IProgrammingLanguageDataAccess programmingLanguageDataAccess)
    : IEmployeeService
{
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await employeeDataAccess.Get(cancellationToken);
        if (!result.IsSuccess)
        {
            return new ErrorActionResult(result.Message!);
        }
        
        return new SuccessActionResult(result.Payload!); 
    }

    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var result = await employeeDataAccess.Get(id, cancellationToken);
        if (!result.IsSuccess)
        {
            return new ErrorActionResult(result.Message!);
        }
        
        return new SuccessActionResult(result.Payload!); 
    }

    public async Task<IActionResult> GetNames(string key, CancellationToken cancellationToken)
    {
        if (key.IsNullOrEmpty())
        {
            return new ErrorActionResult(HttpStatusCode.BadRequest);
        }

        var result = await employeeDataAccess.GetNames(key, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return new ErrorActionResult(result.Message!);
        }
        
        return new SuccessActionResult(result.Payload!); 
    }

    public async Task<IActionResult> Add(EmployeeRequest request, CancellationToken cancellationToken)
    {
        if (!IsValid(request, out var errorMessage))
        {
            return new ErrorActionResult(errorMessage!, HttpStatusCode.BadRequest);
        }
        
        var departmentResult = await departmentDataAccess.Get(request.DepartmentID, cancellationToken);
        if (!departmentResult.IsSuccess)
        {
            return new ErrorActionResult(departmentResult.Message!);
        }

        var programmingLanguageResult = await programmingLanguageDataAccess
            .Any(request.ProgrammingLanguageIDs, cancellationToken);
        
        if (!programmingLanguageResult.IsSuccess)
        {
            return new ErrorActionResult(programmingLanguageResult.Message!);
        }

        if (!programmingLanguageResult.Payload)
        {
            return new ErrorActionResult("Not all found", HttpStatusCode.BadRequest);
        }

        var result = await employeeDataAccess.Add(
            ConvertRequestToModel(request), cancellationToken);

        if (result.IsSuccess)
        {
            return new SuccessActionResult(result.Payload!);
        }

        return new ErrorActionResult(result.Message!);
    }

    public async Task<IActionResult> Edit(EmployeeRequest request, int id, CancellationToken cancellationToken)
    {
        if (!IsValid(request, out var errorMessage))
        {
            return new ErrorActionResult(errorMessage!, HttpStatusCode.BadRequest);
        }
        
        var departmentResult = await departmentDataAccess.Get(request.DepartmentID, cancellationToken);
        if (!departmentResult.IsSuccess)
        {
            return new ErrorActionResult(departmentResult.Message!);
        }

        var programmingLanguageResult = await programmingLanguageDataAccess
            .Any(request.ProgrammingLanguageIDs, cancellationToken);
        if (!programmingLanguageResult.IsSuccess)
        {
            return new ErrorActionResult(programmingLanguageResult.Message!);
        }

        if (!programmingLanguageResult.Payload)
        {
            return new ErrorActionResult("Not all found", HttpStatusCode.BadRequest);
        }

        var result = await employeeDataAccess
            .Edit(ConvertRequestToModel(request), id, cancellationToken);

        if (result.IsSuccess)
        {
            return new SuccessActionResult();
        }

        return new ErrorActionResult(result.Message!);
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await employeeDataAccess.Delete(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return new ErrorActionResult(result.Message!);
        }

        return new SuccessActionResult();
    }

    private static bool IsValid(EmployeeRequest request, out string? errorMessage)
    {
        if (request is null)
        {
            errorMessage = "Model error";
            return false;
        }

        if (request.FirstName.IsNullOrEmpty())
        {
            errorMessage = "Firstname required";
            return false;
        }
        
        if (request.LastName.IsNullOrEmpty())
        {
            errorMessage = "LastName required";
            return false;
        }
        
        if (request.Age <= 0)
        {
            errorMessage = "Age must be greater 0";
            return false;
        }

        if (request.ProgrammingLanguageIDs is null || !request.ProgrammingLanguageIDs.Any())
        {
            errorMessage = "programming language required";
            return false;
        }

        errorMessage = null;
        return true;
    }

    private EmployeeRequestModel ConvertRequestToModel(EmployeeRequest request)
    {
        return new EmployeeRequestModel(
            FirstName: request.FirstName,
            LastName: request.LastName,
            Age: request.Age,
            Gender: request.Gender.ConvertToDbType(),
            DepartmentID: request.DepartmentID,
            ProgrammingLanguageIDs: request.ProgrammingLanguageIDs);
    }
}