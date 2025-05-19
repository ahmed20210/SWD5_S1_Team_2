# Repository and Unit of Work Pattern

This project uses the Repository and Unit of Work patterns to abstract the data access layer and provide a consistent API for working with the database.

## Using the Unit of Work Pattern

The Unit of Work pattern provides a way to group operations into a single transaction and maintain a list of objects affected by a business transaction.

### Dependency Injection

The Unit of Work and repositories are registered in the DI container using the extension method:

```csharp
// In Program.cs
builder.Services.AddRepositories();
```

### Basic Usage

Here's how to use the Unit of Work pattern in your services or controllers:

```csharp
public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Order> GetOrderAsync(int id)
    {
        return await _unitOfWork.Orders.GetOrderWithDetailsAsync(id);
    }

    public async Task CreateOrderAsync(Order order)
    {
        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.CompleteAsync();
    }

    public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order != null)
        {
            order.Status = status;
            
            // Add timeline entry
            var timeline = new OrderTimeLine
            {
                OrderId = orderId,
                Status = status,
                ChangedAt = DateTime.UtcNow,
                Description = $"Order status changed to {status}"
            };
            
            await _unitOfWork.OrderTimeLines.AddAsync(timeline);
            await _unitOfWork.CompleteAsync();
        }
    }
}
```

## Available Repositories

The following repositories are available through the Unit of Work:

- `Orders`: For working with Order entities
- `OrderItems`: For working with OrderItem entities
- `OrderTimeLines`: For working with OrderTimeLine entities
- `Payments`: For working with Payment entities

Each repository provides the standard CRUD operations plus additional specialized methods for the specific entity type.

## Transaction Management

The Unit of Work pattern helps manage transactions. All changes are saved only when the `CompleteAsync()` method is called:

```csharp
// Example of a transaction with multiple operations
await _unitOfWork.Orders.AddAsync(order);
await _unitOfWork.Payments.AddAsync(payment);
await _unitOfWork.OrderTimeLines.AddAsync(timeline);

// Save all changes in a single transaction
await _unitOfWork.CompleteAsync();
```

This ensures that either all operations succeed or none of them do.
