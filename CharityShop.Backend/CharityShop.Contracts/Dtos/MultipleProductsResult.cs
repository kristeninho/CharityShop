namespace CharityShop.Contracts.Dtos;

/// <summary>
/// Represent a result for a multiple products request
/// </summary>
public class MultipleProductsResult
{
    /// <summary>
    /// Products information
    /// </summary>
    public List<ProductResponseDto> Products { get; set; }
    /// <summary>
    /// Health status of the request
    /// </summary>
    public HealthStatus HealthStatus { get; set; } = new ();
}