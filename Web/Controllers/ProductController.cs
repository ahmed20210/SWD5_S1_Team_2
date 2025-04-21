using Business.Services.ProductService;
using Domain.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

   
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO productDto)
    {
        var result = await _productService.CreateProductAsync(productDto);
        return result.Success ? Ok(result) : BadRequest(result);
    }
   
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO productDto)
    {
        var result = await _productService.UpdateProductAsync(id, productDto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateProductStatus(int id, [FromBody] UpdateProductStatusDTO statusDto)
    {
        var result = await _productService.UpdateProductStatusAsync(id, statusDto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

   
    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetProductsByCategory(int categoryId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var result = await _productService.GetProductsByCategoryAsync(categoryId, pageNumber, pageSize);
        return result.Success ? Ok(result) : NotFound(result);
    }

    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var result = await _productService.GetProductByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

   
    [HttpGet("search")]
    public async Task<IActionResult> SearchProducts([FromQuery] string query, [FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var result = await _productService.SearchProductsAsync(query, pageNumber, pageSize);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    
    [HttpGet("filter")]
    public async Task<IActionResult> FilterProducts(
        [FromQuery] int? categoryId,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] string sortBy = "price",  
        [FromQuery] bool ascending = true,    
        [FromQuery] int pageSize = 10)
    {
        var result = await _productService.FilterProductsAsync(categoryId, minPrice, maxPrice, sortBy, ascending, pageNumber, pageSize);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}