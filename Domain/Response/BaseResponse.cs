namespace Domain.Response;

public class BaseResponse : IResponse
{

    public bool Success { get; set; }
    public string? Message { get; set; }
    public object? Errors { get; set; }

    public static BaseResponse SuccessResponse(string message = "Operation completed successfully", int statusCode = 200)
    {
        return new BaseResponse
        {
            Success = true,
            Message = message,
            
        };
    }

    public static BaseResponse FailureResponse(string message, object? errors = null)
    {
        return new BaseResponse
        {
            Success = false,
            Message = message,
            Errors = errors
        };
    }
}
