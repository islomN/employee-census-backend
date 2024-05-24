using System.Net;
using Models;

namespace Api.ActionResults;

public class ErrorActionResult: ExtendedObjectResult
{
    public ErrorActionResult(
        string message,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError) 
        : base(
            new ErrorResultModel(message),
            statusCode)
    {
            
    }
        
    public ErrorActionResult(
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError) 
        : base(
            new ErrorResultModel(null),
            statusCode)
    {
            
    }

    public ErrorActionResult(
        object value,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError) 
        : base(value, statusCode)
    {
            
    }
}