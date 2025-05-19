namespace Business.Services.PaymentService;

using Business.ViewModels.PaymentViewModels;
public interface IPaymentServiceUsingUnitOfWork
{
    Task<IEnumerable<PaymentViewModel>> GetAllAsync();
    Task CreateAsync(CreatePaymentViewModel model);
    Task<PaymentViewModel?> GetByIdAsync(int id);
    Task<PaymentViewModel?> EditAsync(int id, UpdateViewModel model);
    Task<UpdateViewModel?> GetByIdForUpdateAsync(int id);
    Task<bool> DeleteAsync(int id);
    Task<RefundResult> RefundPaymentAsync(string transactionId, decimal amount);
    Task<PaymentResult> ProcessAndVerifyPaymentAsync(CreatePaymentViewModel model);
}


public record RefundResult(
    string? RefundId = null,
    decimal AmountRefunded = 0,
    string? ErrorMessage = null)
{
    public static RefundResult Success(string refundId, decimal amountRefunded)
        => new(refundId, amountRefunded);

    public static RefundResult Failed(string error)
        => new(ErrorMessage: error);
}
