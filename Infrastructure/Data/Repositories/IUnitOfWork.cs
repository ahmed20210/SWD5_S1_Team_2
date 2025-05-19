namespace Infrastructure.Data.Repositories;

public interface IUnitOfWork : IDisposable
{
    IOrderRepository Orders { get; }
    IOrderItemRepository OrderItems { get; }
    IOrderTimeLineRepository OrderTimeLines { get; }
    IPaymentRepository Payments { get; }
    
    Task<int> CompleteAsync();
}
