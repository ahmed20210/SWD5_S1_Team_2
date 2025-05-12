namespace Business.Services.PaymentService;

public interface IPaymentGatewayService
{
    Task<GatewayPaymentResponse> ProcessPaymentAsync(PaymentRequest request);
    Task<GatewayVerificationResponse> VerifyPaymentAsync(string transactionId);
    Task<GatewayRefundResponse> RefundPaymentAsync(string transactionId, decimal amount);
}

public record PaymentRequest(
    decimal Amount,
    string Currency,
    string CustomerEmail,
    string Description,
    Dictionary<string, string> Metadata = null);

public record GatewayPaymentResponse(
    bool Success,
    string TransactionId,
    string GatewayReference,
    string Status,
    DateTime ProcessedAt,
    string ErrorMessage = null);

public record GatewayVerificationResponse(
    bool IsValid,
    decimal Amount,
    string Currency,
    string Status,
    DateTime PaymentDate);

public record GatewayRefundResponse(
    bool Success,
    string RefundId = null,
    string Status = null,
    decimal? AmountRefunded = 0,
    string ErrorMessage = null
);
public record PaymentResult(string Message, int? PaymentId = null)
{
    public static PaymentResult Success(int paymentId) 
        => new("Payment processed successfully", paymentId);
    
    public static PaymentResult Failed(string message) 
        => new(message);
}