using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Services.AddressService;
using Business.Services.OrderItemService;
using Business.Services.OrderService;
using Business.Services.OrderTimeLineService;
using Business.Services.PaymentService;
using Business.ViewModels.CheckoutViewModels;
using Business.ViewModels.OrderItemViewModel;
using Domain;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Business.Services.ChechoutService;

public class CheckoutService : ICheckoutService
{
    private readonly IOrderServiceUsingUnitOfWork _orderService;

    private readonly IAddressService _addressService;
    private readonly IPaymentServiceUsingUnitOfWork _paymentService;
    private readonly IOrderItemServiceWithUnitOfWork _orderItemService;
    private readonly IOrderTimeLineService _orderTimeLineService;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CheckoutService> _logger;

    public CheckoutService(
        IOrderServiceUsingUnitOfWork orderService,
        IPaymentServiceUsingUnitOfWork paymentService,
        IOrderItemServiceWithUnitOfWork orderItemService,
        IOrderTimeLineService orderTimeLineService,
        IAddressService addressService,
        ApplicationDbContext dbContext,
        ILogger<CheckoutService> logger)
    {
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        _orderItemService = orderItemService ?? throw new ArgumentNullException(nameof(orderItemService));
        _orderTimeLineService = orderTimeLineService ?? throw new ArgumentNullException(nameof(orderTimeLineService));
        _addressService = addressService ?? throw new ArgumentNullException(nameof(addressService));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<CheckoutResult> ProcessCheckoutAsync(CheckoutViewModel model)
    {
        // Validate checkout data
        var validationResult = ValidateCheckoutModel(model);
        if (!validationResult.IsSuccess)
            return validationResult;


        // Use a transaction for the entire checkout process to ensure data consistency
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            if (model.ShippingAddress == null)
            {
                //get user main address from user.mainAddress and get user from userId
                var user = await _dbContext.Users.FindAsync(model.Order.CustomerId);
                if (user.MainAddressId == null)
                {
                    _logger.LogError("User {UserId} does not have a main address", model.Order.CustomerId);
                    await transaction.RollbackAsync();
                    return CheckoutResult.Failed("User does not have a main address add your main address or select another address");
                }

               
            }

            // create a new address
            var address = await _addressService.CreateAddressAsync(model.ShippingAddress, model.Order.CustomerId);


            model.Order.AddressId = address.Data.Id;


            // 1. Create order
            int orderId = await _orderService.CreateOrderAsync(model.Order);
            if (orderId <= 0)
            {
                _logger.LogError("Failed to create order during checkout");
                await transaction.RollbackAsync();
                return CheckoutResult.Failed("Failed to create order");
            }

            // Set the order ID in the payment model
            model.Payment.OrderId = orderId;

            // 2. Add initial timeline entry
            await _orderTimeLineService.AddTimeLineEntryAsync(
                orderId,
                OrderStatus.Pending,
                "Order created, processing items");

            // 3. Convert and create order items
            var orderItems = model.Order.Items.Select(item => new CreateRangeOrderItemsVM
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.UnitPrice
            }).ToList();

            var orderItemsResult = await _orderItemService.CreateRangeAsync(
                orderItems,
                orderId,
                model.Order.TotalAmount);

            if (!orderItemsResult)
            {
                _logger.LogError("Failed to create order items for order {OrderId}", orderId);
                await _orderTimeLineService.AddTimeLineEntryAsync(
                    orderId,
                    OrderStatus.Cancelled,
                    "Failed to create order items");

                await transaction.RollbackAsync();
                return CheckoutResult.Failed("Failed to create order items");
            }

            // 4. Process and verify payment
            await _orderTimeLineService.AddTimeLineEntryAsync(
                orderId,
                OrderStatus.Pending,
                "Order items created, processing payment");

            var paymentResult = await _paymentService.ProcessAndVerifyPaymentAsync(model.Payment);
            if (paymentResult.PaymentId == null)
            {
                _logger.LogError("Payment failed for order {OrderId}: {Message}", orderId, paymentResult.Message);

                await _orderTimeLineService.AddTimeLineEntryAsync(
                    orderId,
                    OrderStatus.Cancelled,
                    $"Payment failed: {paymentResult.Message}");

                await transaction.RollbackAsync();
                return CheckoutResult.Failed($"Payment failed: {paymentResult.Message}");
            }

            // 5. Add payment success timeline entry
            await _orderTimeLineService.AddTimeLineEntryAsync(
                orderId,
                OrderStatus.Processing,
                "Payment processed successfully");

            // 6. Complete the order
            var completionResult = await _orderService.CompleteOrderAsync(orderId);
            if (!completionResult.Success)
            {
                _logger.LogError("Order completion failed for order {OrderId}: {Message}", orderId, completionResult.Message);

                await _orderTimeLineService.AddTimeLineEntryAsync(
                    orderId,
                    OrderStatus.Cancelled,
                    $"Order completion failed: {completionResult.Message}");

                await transaction.RollbackAsync();
                return CheckoutResult.Failed($"Order completion failed: {completionResult.Message}");
            }

            // 7. Add order completion timeline entry
            await _orderTimeLineService.AddTimeLineEntryAsync(
                orderId,
                OrderStatus.Shipped,
                "Order completed successfully");

            // 8. Commit the transaction
            await transaction.CommitAsync();

            _logger.LogInformation("Checkout completed successfully for order {OrderId}", orderId);
            return CheckoutResult.Success(orderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing checkout");

            // Ensure transaction is rolled back
            try
            {
                await transaction.RollbackAsync();
            }
            catch (Exception rollbackEx)
            {
                _logger.LogError(rollbackEx, "Error during transaction rollback");
            }

            return CheckoutResult.Failed($"Checkout processing error: {ex.Message}");
        }
    }

    private CheckoutResult ValidateCheckoutModel(CheckoutViewModel model)
    {
        // Base validation
        if (model == null)
            return CheckoutResult.Failed("Checkout data is missing");

        if (model.Order == null)
            return CheckoutResult.Failed("Order data is missing");

        if (model.Payment == null)
            return CheckoutResult.Failed("Payment data is missing");

        if (model.Order.Items == null || model.Order.Items.Count == 0)
            return CheckoutResult.Failed("Order items are missing");

        // Order validation
        if (string.IsNullOrWhiteSpace(model.Order.CustomerId))
            return CheckoutResult.Failed("Customer ID is required");

        if (model.Order.TotalAmount <= 0)
            return CheckoutResult.Failed("Total amount must be greater than zero");

        // Payment validation
        if (model.Payment.Amount <= 0)
            return CheckoutResult.Failed("Payment amount must be greater than zero");

        if (model.Payment.Amount != model.Order.TotalAmount)
            return CheckoutResult.Failed($"Payment amount ({model.Payment.Amount}) doesn't match order total ({model.Order.TotalAmount})");

        if (string.IsNullOrWhiteSpace(model.Payment.TransactionId))
            return CheckoutResult.Failed("Transaction ID is required");

        // Order items validation
        foreach (var item in model.Order.Items)
        {
            if (item.ProductId <= 0)
                return CheckoutResult.Failed($"Invalid product ID: {item.ProductId}");

            if (item.Quantity <= 0)
                return CheckoutResult.Failed($"Invalid quantity for product {item.ProductId}: {item.Quantity}");

            if (item.UnitPrice <= 0)
                return CheckoutResult.Failed($"Invalid unit price for product {item.ProductId}: {item.UnitPrice}");
        }

        // Verify total amount matches sum of items
        decimal calculatedTotal = model.Order.Items.Sum(item => item.UnitPrice * item.Quantity);
        if (Math.Abs(calculatedTotal - model.Order.TotalAmount) > 0.01m)
            return CheckoutResult.Failed($"Order total amount ({model.Order.TotalAmount}) doesn't match calculated sum of items ({calculatedTotal})");

        return CheckoutResult.Success(0); // Temporary ID, will be replaced by actual order ID
    }
}
