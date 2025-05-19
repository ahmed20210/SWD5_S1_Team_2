using Business.ViewModels.OrderItemViewModel;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Business.Services.OrderItemService
{
    public class OrderItemServiceWithUnitOfWork : IOrderItemServiceWithUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<OrderItemServiceWithUnitOfWork> _logger;

        public OrderItemServiceWithUnitOfWork(
            IUnitOfWork unitOfWork,
            ApplicationDbContext dbContext,
            ILogger<OrderItemServiceWithUnitOfWork> logger)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<OrderItem> CreateAsync(OrderItem orderItem)
        {
            try
            {
                await _unitOfWork.OrderItems.AddAsync(orderItem);
                await _unitOfWork.CompleteAsync();
                return orderItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order item for OrderId: {OrderId}, ProductId: {ProductId}",
                    orderItem.OrderId, orderItem.ProductId);
                throw;
            }
        }
        
        public async Task<bool> CreateRangeAsync(List<CreateRangeOrderItemsVM> items, int orderId, decimal totalAmountConfirmed)
        {
            try
            {
                // Get product IDs to fetch from DB
                var productIds = items.Select(i => i.ProductId).ToList();
                
                // Fetch all products in one query
                var products = await _dbContext.Products
                    .Where(p => productIds.Contains(p.Id))
                    .Include(p => p.Discount)
                    .ToListAsync();
                var productsList = products.ToList();
                
                // Check if all products exist
                if (productsList.Count != productIds.Count)
                {
                    _logger.LogWarning("Some products in order were not found");
                    return false;
                }
                
                // Create order items
                var orderItems = new List<OrderItem>();
                decimal totalAmount = 0;
                foreach (var item in items)
                {
                    
                    
                    var product = productsList.First(p => p.Id == item.ProductId);
                    if (item.Quantity <= 0)
                    {
                        _logger.LogWarning("Invalid quantity for product in order for product : {name}", product.Name);
                        return false;
                    }
                    // Calculate price (use provided price or product price)
                    var unitPrice = product.Price;
                    
                    // Get applicable discount if any
                    var currentDate = DateTime.UtcNow;
                    var discount = product.Discount != null && 
                                  product.Discount.StartDate <= currentDate && 
                                  product.Discount.EndDate >= currentDate 
                                  ? product.Discount 
                                  : null;
                    
                    // Calculate discounted price if discount applies
                    if (discount != null)
                    {
                        unitPrice = Math.Round(unitPrice - (unitPrice * discount.Amount / 100), 2);
                    }
                    
                    var orderItem = new OrderItem
                    {
                        OrderId = orderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = unitPrice,
                        TotalAmount = unitPrice * item.Quantity,
                        DiscountId = discount?.Id
                    };
                    totalAmount += orderItem.TotalAmount;
                    
                    orderItems.Add(orderItem);
                }
                // to int
                if ((int)totalAmount != (int)totalAmountConfirmed)
                {
                    _logger.LogWarning("Total amount mismatch for order items");
                    return false;
                }
                
                
                

                // Save all order items in one operation
                await _unitOfWork.OrderItems.AddRangeAsync(orderItems);
                await _unitOfWork.CompleteAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order items for OrderId: {OrderId}", orderId);
                throw;
            }
        }

        

        public async Task<OrderItem> GetByIdAsync(int orderId, int productId)
        {
            try
            {
                var orderItems = await _unitOfWork.OrderItems.FindAsync(oi =>
                    oi.OrderId == orderId && oi.ProductId == productId);
                return orderItems.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order item for OrderId: {OrderId}, ProductId: {ProductId}",
                    orderId, productId);
                throw;
            }
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            try
            {
                return await _unitOfWork.OrderItems.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all order items");
                throw;
            }
        }

        public async Task<OrderItem> UpdateAsync(OrderItem orderItem)
        {
            try
            {
                _unitOfWork.OrderItems.Update(orderItem);
                await _unitOfWork.CompleteAsync();
                return orderItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order item for OrderId: {OrderId}, ProductId: {ProductId}",
                    orderItem.OrderId, orderItem.ProductId);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int orderId, int productId)
        {
            try
            {
                var orderItem = await GetByIdAsync(orderId, productId);
                if (orderItem != null)
                {
                    _unitOfWork.OrderItems.Remove(orderItem);
                    await _unitOfWork.CompleteAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order item for OrderId: {OrderId}, ProductId: {ProductId}",
                    orderId, productId);
                throw;
            }
        }

        public async Task<IEnumerable<OrderItemViewModel>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            try
            {
                var orderItems = await _unitOfWork.OrderItems.GetOrderItemsByOrderIdAsync(orderId);
                
                return orderItems.Select(oi => new OrderItemViewModel
                {
                    OrderId = oi.OrderId,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    TotalAmount = oi.TotalAmount,
                    DiscountId = oi.DiscountId,
                    DiscountAmount = oi.Discount?.Amount ?? 0
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order items view model for OrderId: {OrderId}", orderId);
                throw;
            }
        }
    }
}
