using AutoMapper;
using Business.ViewModels.PaymentViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using AutoMapper;
using Business.ViewModels.PaymentViewModels;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Business.Services.OrderService;
using Domain;
using Microsoft.Extensions.Logging;

namespace Business.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPaymentGatewayService _gateway;
        private readonly ILogger<PaymentService> _logger;
        private readonly IOrderService _orderService;
        public PaymentService(ApplicationDbContext context, IMapper mapper ,  IOrderService orderService, ILogger<PaymentService>  logger , IPaymentGatewayService  gatway)
        {
            _context = context;
            _mapper = mapper;
            _gateway = gatway;
            _logger = logger;   
            _orderService = orderService;
        }
        public async Task<RefundResult> RefundPaymentAsync(string transactionId, decimal amount)
        {
            // 1. Validate payment exists
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.TransactionId == transactionId);

            if (payment == null || payment.Status != PaymentStatus.Success)
                return RefundResult.Failed("Invalid payment for refund");

            // 2. Process refund
            var refundResponse = await _gateway.RefundPaymentAsync(transactionId, amount);

            if (!refundResponse.Success)
                return RefundResult.Failed(refundResponse.ErrorMessage);

            // 3. Update payment record (with null check)
            payment.Status = PaymentStatus.Refunded;
            payment.Amount = refundResponse.AmountRefunded ?? amount; // Fallback to original amount
            payment.CreatedAt= DateTime.UtcNow;
    
            await _context.SaveChangesAsync();

            return RefundResult.Success(
                refundId: refundResponse.RefundId,
                amountRefunded: refundResponse.AmountRefunded ?? amount
            );
        }
        public async Task<IEnumerable<PaymentViewModel>> GetAllAsync()
        {
            var payments = await _context.Payments.ToListAsync();
            return payments.Select(p => new PaymentViewModel
            {
                TransactionId = p.TransactionId,
                Date = p.CreatedAt,
                Status = p.Status,
                Amount = p.Amount,
                Method = p.Method
            });
        }


        public async Task CreateAsync(CreatePaymentViewModel model)
        {
            var payment = new Payment
            {
                TransactionId = model.TransactionId,
                OrderId = model.OrderId,
                Amount = model.Amount,
                Status = model.Status,
                Method = model.Method,
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }
        public async Task<UpdateViewModel> GetByIdForUpdateAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return null;

            return new UpdateViewModel
            {
                Id = payment.Id,
                TransactionId = payment.TransactionId,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                Method = payment.Method,
                Status = payment.Status
            };
        }

        public async Task<PaymentViewModel> GetByIdAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return null;

            return new PaymentViewModel
            {
                Id = payment.Id,
                TransactionId = payment.TransactionId,
                Date = payment.CreatedAt,
                Amount = payment.Amount,
                Method = payment.Method,
                Status = payment.Status
            };
        }

    public async Task<PaymentResult> ProcessAndVerifyPaymentAsync(CreatePaymentViewModel model)
    {
        try
        {
            // 1. Verify with payment gateway
            var verification = await _gateway.VerifyPaymentAsync(model.TransactionId);
            
            if (!verification.IsValid)
                return PaymentResult.Failed("Payment verification failed");
            
            if (verification.Amount != model.Amount)
                return PaymentResult.Failed("Amount mismatch");
            
            // 2. Check for duplicate payment
            var existingPayment = await _context.Payments
                .FirstOrDefaultAsync(p => p.TransactionId == model.TransactionId);
            
            if (existingPayment != null)
                return PaymentResult.Failed("Duplicate payment detected");
            
            // 3. Create payment record
            var payment = new Payment
            {
                TransactionId = model.TransactionId,
                OrderId = model.OrderId,
                Amount = model.Amount,
                Status = PaymentStatus.Success,
                Method = model.Method,
                CreatedAt = DateTime.UtcNow
            };
            
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            
            // 4. Complete the order
            var orderResult = await _orderService.CompleteOrderAsync(model.OrderId);
            if (!orderResult.Success)
            {
                // Compensating transaction
                payment.Status = PaymentStatus.Failed;
                await _context.SaveChangesAsync();
                return PaymentResult.Failed($"Order processing failed");
            }
            
            return PaymentResult.Success(payment.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Payment processing failed");
            return PaymentResult.Failed("System error during payment processing");
        }
    }



        public async Task<PaymentViewModel> EditAsync(int id, UpdateViewModel model)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return null;
            }

            payment.Amount = model.Amount;
            payment.Status = model.Status;
            payment.Method = model.Method;

            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();

            return new PaymentViewModel
            {
                Id = payment.Id,
                TransactionId = payment.TransactionId,
                OrderId = payment.OrderId,
                Date = payment.CreatedAt,
                Amount = payment.Amount,
                Status = payment.Status,
                Method = payment.Method
            };
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return false;
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

