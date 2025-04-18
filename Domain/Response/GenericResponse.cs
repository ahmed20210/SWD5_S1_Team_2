namespace Domain.Response;

public class GenericResponse<T> : IResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public int? TotalCount { get; set; }
    public int? CurrentPage { get; set; }
    public int? TotalPages { get; set; }
    public object? Errors { get; set; }

    public static GenericResponse<T> SuccessResponse(
        T data,
        string message = "Operation completed successfully",
        int? totalCount = null,
        int? currentPage = null,
        int? totalPages = null)
    {
        return new GenericResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            TotalCount = totalCount,
            CurrentPage = currentPage,
            TotalPages = totalPages
        };
    }

    public static GenericResponse<T> FailureResponse(string message, object? errors = null)
    {
        return new GenericResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors
        };
    }
}