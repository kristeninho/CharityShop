using CharityShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CharityShop.WebAPI.Hubs;
/// <summary>
/// Hub for Product SignalR messaging methods
/// </summary>
[Authorize]
public class ProductHub(IProductService productService) : Hub
{
    /// <summary>
    /// Book a single Product
    /// </summary>
    /// <param name="productId">Id of Product to be booked</param>
    public async Task BookProduct(string productId)
    {
        if (int.TryParse(productId, out int intProductId))
        {
            var singleProductResult = await productService.BookProductAsync(intProductId);
            await Clients.All.SendAsync(Common.ProductHub.UpdateSingleProductHubMethod, singleProductResult); 
        }
    }
}