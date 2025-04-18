namespace Domain.Response;

public interface IResponse
{
    bool Success { get; }
    string? Message { get; }
    object? Errors { get; }
}