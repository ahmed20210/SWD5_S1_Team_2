using Domain.Entities;
using Infrastructure.Data.Repositories;
using Microsoft.Extensions.Logging;
using Domain;

namespace Business.Services.OrderTimeLineService
{
   
    public class OrderTimeLineService : IOrderTimeLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderTimeLineService> _logger;

        public OrderTimeLineService(
            IUnitOfWork unitOfWork,
            ILogger<OrderTimeLineService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<OrderTimeLine> AddTimeLineEntryAsync(int orderId, OrderStatus status, string description)
        {
            try
            {
                var timeLineEntry = new OrderTimeLine
                {
                    OrderId = orderId,
                    Status = status,
                    ChangedAt = DateTime.UtcNow,
                    Description = description
                };

                await _unitOfWork.OrderTimeLines.AddAsync(timeLineEntry);
                await _unitOfWork.CompleteAsync();

                return timeLineEntry;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding timeline entry for order {OrderId}", orderId);
                throw;
            }
        }

        public async Task<IEnumerable<OrderTimeLine>> GetTimeLineByOrderIdAsync(int orderId)
        {
            try
            {
                return await _unitOfWork.OrderTimeLines.GetTimeLineByOrderIdAsync(orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving timeline for order {OrderId}", orderId);
                throw;
            }
        }

        public async Task<OrderTimeLine> GetLatestStatusForOrderAsync(int orderId)
        {
            try
            {
                return await _unitOfWork.OrderTimeLines.GetLatestStatusForOrderAsync(orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving latest status for order {OrderId}", orderId);
                throw;
            }
        }
    }
}
