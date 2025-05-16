using Business.Services.DiscountService;
using Domain.DTOs.DiscountDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountService _discountService;

    public DiscountController(IDiscountService discountService)
    {
        _discountService = discountService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDiscount([FromBody] CreateDiscountDTO discountDto)
    {

        var result = await _discountService.CreateDiscountAsync(discountDto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDiscount(int id, [FromBody] UpdateDiscountDTO discountDto)
    {
        var result = await _discountService.UpdateDiscountAsync(discountDto, id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDiscountById(int id)
    {
        var result = await _discountService.GetDiscountByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDiscounts([FromQuery] int? productId, [FromQuery] bool? isExpired, [FromQuery] List<DateTime>? betweenDates, [FromQuery] int pageNumber=1, [FromQuery] int pageSize=10 )
    {
        var result = await _discountService.GetAllDiscountsAsync( productId, isExpired, betweenDates, pageNumber, pageSize);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("bulk-random")]
    public async Task<IActionResult> CreateBulkRandomDiscounts([FromQuery] int count = 100, 
        [FromQuery] decimal minAmount = 5, [FromQuery] decimal maxAmount = 30,
        [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        // Set default dates if not provided
        var start = startDate ?? DateTime.UtcNow;
        var end = endDate ?? DateTime.UtcNow.AddMonths(1);
        
        var result = await _discountService.CreateBulkRandomDiscountsAsync(count, minAmount, maxAmount, start, end);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("synchronize")]
    public async Task<IActionResult> SynchronizeDiscounts()
    {
        var result = await _discountService.SynchronizeDiscountsAsync();
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDiscount(int id)
    {
        var result = await _discountService.DeleteDiscountAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }
}
