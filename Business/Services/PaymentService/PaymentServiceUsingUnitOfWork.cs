using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.ViewModels.PaymentViewModels;
using Domain;
using Domain.Entities;
using Infrastructure.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services.PaymentService
{
    public class PaymentServiceUsingUnitOfWork : IPaymentServiceUsingUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentServiceUsingUnitOfWork> _logger;
        private readonly IPaymentGatewayService _paymentGateway;

        public PaymentServiceUsingUnitOfWork(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<PaymentServiceUsingUnitOfWork> logger,
            IPaymentGatewayService paymentGateway)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _paymentGateway = paymentGateway ?? throw new ArgumentNullException(nameof(paymentGateway));
        }

        public async Task<IEnumerable<PaymentViewModel>> GetAllAsync()
        {
            try
            {
                var payments = await _unitOfWork.Payments.GetAllAsync();
                var viewModels = new List<PaymentViewModel>();

                foreach (var payment in payments)
                {
                    viewModels.Add(new PaymentViewModel
                    {
                        Id = payment.Id,
                        TransactionId = payment.TransactionId,
                        OrderId = payment.OrderId,
                        Status = payment.Status,
                        Date = payment.CreatedAt,
                        Amount = payment.Amount,
                        Method = payment.Method
                    });
                }

                return viewModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all payments");
                throw;
            }
        }

        public async Task CreateAsync(CreatePaymentViewModel model)
        {
            try
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

                await _unitOfWork.Payments.AddAsync(payment);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment: {TransactionId}", model.TransactionId);
                throw;
            }
        }

        public async Task<PaymentViewModel?> GetByIdAsync(int id)
        {
            try
            {
                var payment = await _unitOfWork.Payments.GetByIdAsync(id);
                if (payment == null)
                    return null;

                return new PaymentViewModel
                {
                    Id = payment.Id,
                    TransactionId = payment.TransactionId,
                    OrderId = payment.OrderId,
                    Status = payment.Status,
                    Date = payment.CreatedAt,
                    Amount = payment.Amount,
                    Method = payment.Method
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment by ID: {Id}", id);
                throw;
            }
        }

        public async Task<PaymentViewModel?> EditAsync(int id, UpdateViewModel model)
        {
            try
            {
                var payment = await _unitOfWork.Payments.GetByIdAsync(id);
                if (payment == null)
                    return null;

                payment.TransactionId = model.TransactionId;
                payment.OrderId = model.OrderId;
                payment.Amount = model.Amount;
                payment.Status = model.Status;
                payment.Method = model.Method;

                _unitOfWork.Payments.Update(payment);
                await _unitOfWork.CompleteAsync();

                return new PaymentViewModel
                {
                    Id = payment.Id,
                    TransactionId = payment.TransactionId,
                    OrderId = payment.OrderId,
                    Status = payment.Status,
                    Date = payment.CreatedAt,
                    Amount = payment.Amount,
                    Method = payment.Method
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment: {Id}", id);
                throw;
            }
        }

        public async Task<UpdateViewModel?> GetByIdForUpdateAsync(int id)
        {
            try
            {
                var payment = await _unitOfWork.Payments.GetByIdAsync(id);
                if (payment == null)
                    return null;

                return new UpdateViewModel
                {
                    Id = payment.Id,
                    TransactionId = payment.TransactionId,
                    OrderId = payment.OrderId,
                    Amount = payment.Amount,
                    Status = payment.Status,
                    Method = payment.Method
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment by ID for update: {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var payment = await _unitOfWork.Payments.GetByIdAsync(id);
                if (payment == null)
                    return false;

                _unitOfWork.Payments.Remove(payment);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting payment: {Id}", id);
                throw;
            }
        }

        public async Task<RefundResult> RefundPaymentAsync(string transactionId, decimal amount)
        {
            try
            {
                // Find the payment by transaction ID
                var payment = await _unitOfWork.Payments.GetPaymentByTransactionIdAsync(transactionId);
                if (payment == null || payment.Status != PaymentStatus.Success)
                {
                    return RefundResult.Failed("Payment not found or not eligible for refund");
                }

                // Validate refund amount
                if (amount <= 0 || amount > payment.Amount)
                {
                    return RefundResult.Failed("Invalid refund amount");
                }

                // Process refund with payment gateway
                var refundResponse = await _paymentGateway.RefundPaymentAsync(transactionId, amount);
                if (!refundResponse.Success)
                {
                    return RefundResult.Failed(refundResponse.ErrorMessage ?? "Refund processing failed");
                }

                // Update payment status
                payment.Status = PaymentStatus.Refunded;
                _unitOfWork.Payments.Update(payment);
                await _unitOfWork.CompleteAsync();

                return RefundResult.Success(
                    refundId: refundResponse.RefundId ?? "unknown",
                    amountRefunded: refundResponse.AmountRefunded ?? amount
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing refund for transaction: {TransactionId}", transactionId);
                return RefundResult.Failed($"System error during refund: {ex.Message}");
            }
        }

        public async Task<PaymentResult> ProcessAndVerifyPaymentAsync(CreatePaymentViewModel model)
        {
            try
            {
                // Verify the payment with the gateway
                var verification = await _paymentGateway.VerifyPaymentAsync(model.TransactionId);
                if (!verification.IsValid)
                {
                    return PaymentResult.Failed("Payment verification failed");
                }

                // Check for amount mismatch
                if (verification.Amount != model.Amount)
                {
                    return PaymentResult.Failed($"Amount mismatch: Expected {model.Amount}, got {verification.Amount}");
                }

                // Check for duplicate payment
                var existingPayment = await _unitOfWork.Payments.GetPaymentByTransactionIdAsync(model.TransactionId);
                if (existingPayment != null)
                {
                    return PaymentResult.Failed("Duplicate payment detected");
                }

                // Create new payment record
                var payment = new Payment
                {
                    TransactionId = model.TransactionId,
                    OrderId = model.OrderId,
                    Amount = model.Amount,
                    Status = PaymentStatus.Success,
                    Method = model.Method,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Payments.AddAsync(payment);
                await _unitOfWork.CompleteAsync();

                return PaymentResult.Success(payment.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment: {TransactionId}", model.TransactionId);
                return PaymentResult.Failed($"System error during payment processing: {ex.Message}");
            }
        }
    }
}
