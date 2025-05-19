namespace Business.ViewModels.PaymentViewModels;

// PaymentIntentResult
public record PaymentIntentResult
{
    public string ClientSecret { get; init; }
    public string PaymentIntentId { get; init; }
    public bool Success { get; init; }
    public string ErrorMessage { get; init; }
    public bool RequiresAction { get; init; }
}

