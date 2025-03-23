using CharityShop.Contracts.Dtos;
using CharityShop.Services;
using CharityShop.WebAPI.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CharityShop.WebAPI.Controllers;
/// <summary>
/// Controller for Product endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController(IProductService productService, IHubContext<ProductHub> productHub) : ControllerBase
{
    /// <summary>
    /// Get all Products
    /// </summary>
    /// <response code="200"></response>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(MultipleProductsResult),StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllProducts()
    {
        var productResponseDtos = await productService.GetAllProductsAsync();
        
        return Ok(productResponseDtos);
    }
    
    /// <summary>
    /// Release stock of Products on "reset"
    /// </summary>
    /// <param name="productsRequestDtos">Products to be released</param>
    /// <returns></returns>
    [HttpPost("release")]
    public async Task<IActionResult> ReleaseProducts([FromBody] List<ProductRequestDto> productsRequestDtos)
    {
        var multipleProductsResult = await productService.ReleaseProductsAsync(productsRequestDtos);
        
        await productHub.Clients.All.SendAsync(Common.ProductHub.UpdateMultipleProductsHubMethod, multipleProductsResult);
        
        return Ok("Products released");
    }
    
    /// <summary>
    /// Finalize stock of Products on "checkout"
    /// </summary>
    /// <param name="productsRequestDtos">Products to be finalized</param>
    /// <returns></returns>
    [HttpPost("finalize")]
    public async Task<IActionResult> FinalizeProducts([FromBody] List<ProductRequestDto> productsRequestDtos)
    {
        var multipleProductsResult = await productService.FinalizeProductsAsync(productsRequestDtos);
        
        await productHub.Clients.All.SendAsync(Common.ProductHub.UpdateMultipleProductsHubMethod, multipleProductsResult);
        
        return Ok("Products finalized");
    }
    
    /// <summary>
    /// Initialize stock of Products for type Items
    /// </summary>
    /// <param name="productsRequestDtos">Products to be initialized. Must be of type Items</param>
    /// <returns></returns>
    [HttpPost("initialize")]
    public async Task<IActionResult> InitializeItemsStock([FromBody] List<ProductRequestDto> productsRequestDtos)
    {
        await productService.InitializeItemsStockAsync(productsRequestDtos);
        
        return Ok("Stock Initialized");
    }
}