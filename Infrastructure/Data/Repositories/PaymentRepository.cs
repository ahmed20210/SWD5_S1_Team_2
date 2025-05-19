using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public interface IPaymentRepository : IRepository<Payment>
{
    Task<Payment> GetPaymentByTransactionIdAsync(string transactionId);
    Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status);
    Task<Payment> GetPaymentByOrderIdAsync(int orderId);
}

public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    public PaymentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Payment> GetPaymentByTransactionIdAsync(string transactionId)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(p => p.TransactionId == transactionId);
    }

    public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status)
    {
        return await _context.Payments
            .Where(p => p.Status == status)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<Payment> GetPaymentByOrderIdAsync(int orderId)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(p => p.OrderId == orderId);
    }
}
