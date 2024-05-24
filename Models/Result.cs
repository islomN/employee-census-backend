namespace Models;

public class Result(string? message = null, bool isSuccess = true)
{
    public bool IsSuccess { get; init; } = isSuccess;
    
    public string? Message { get; init; } = message;
}

public class Result<T>(
    T? payload,
    string? message = null,
    bool isSuccess = true)
    : Result(message, isSuccess)
{
    public T? Payload { get; init; } = payload;
}