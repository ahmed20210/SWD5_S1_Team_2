namespace Domain.Response;

public class ResponseHandler
{
    #region Base Response Methods
    
    public static BaseResponse Success(string message = "Operation completed successfully")
    {
        return BaseResponse.SuccessResponse(message);
    }
    
    public static BaseResponse Error(string message, object? errors = null)
    {
        return BaseResponse.FailureResponse(message, errors);
    }
    
    public static BaseResponse NotFound(string message = "Resource not found")
    {
        return BaseResponse.FailureResponse(message);
    }
    
    public static BaseResponse BadRequest(string message = "Invalid request", object? errors = null)
    {
        return BaseResponse.FailureResponse(message, errors);
    }
    
    public static BaseResponse Unauthorized(string message = "Unauthorized access")
    {
        return BaseResponse.FailureResponse(message);
    }
    
    public static BaseResponse Forbidden(string message = "Access forbidden")
    {
        return BaseResponse.FailureResponse(message);
    }
    
    #endregion
    
    #region Generic Response Methods
    
    public static GenericResponse<T> Success<T>(T data, string message = "Operation completed successfully")
    {
        return GenericResponse<T>.SuccessResponse(data, message);
    }
    
    public static GenericResponse<T> Success<T>(T data, int totalCount, int currentPage, int totalPages, string message = "Operation completed successfully")
    {
        return GenericResponse<T>.SuccessResponse(data, message, totalCount, currentPage, totalPages);
    }
    
    public static GenericResponse<T> Error<T>(string message, object? errors = null)
    {
        return GenericResponse<T>.FailureResponse(message, errors);
    }
    
    public static GenericResponse<T> NotFound<T>(string message = "Resource not found")
    {
        return GenericResponse<T>.FailureResponse(message);
    }
    
    public static GenericResponse<T> BadRequest<T>(string message = "Invalid request", object? errors = null)
    {
        return GenericResponse<T>.FailureResponse(message, errors);
    }
    
    public static GenericResponse<T> Unauthorized<T>(string message = "Unauthorized access")
    {
        return GenericResponse<T>.FailureResponse(message);
    }
    
    public static GenericResponse<T> Forbidden<T>(string message = "Access forbidden")
    {
        return GenericResponse<T>.FailureResponse(message);
    }
    
    #endregion
}
