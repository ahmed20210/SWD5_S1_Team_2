namespace Infrastructure.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IOrderRepository _orderRepository;
    private IOrderItemRepository _orderItemRepository;
    private IOrderTimeLineRepository _orderTimeLineRepository;
    private IPaymentRepository _paymentRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IOrderRepository Orders => _orderRepository ??= new OrderRepository(_context);
    public IOrderItemRepository OrderItems => _orderItemRepository ??= new OrderItemRepository(_context);
    public IOrderTimeLineRepository OrderTimeLines => _orderTimeLineRepository ??= new OrderTimeLineRepository(_context);
    public IPaymentRepository Payments => _paymentRepository ??= new PaymentRepository(_context);

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
