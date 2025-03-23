namespace CharityShop.Contracts.Dtos;

/// <summary>
/// Represent a result for a single product request
/// </summary>
public class SingleProductResult
{
    /// <summary>
    /// Product information
    /// </summary>
    public ProductResponseDto Product { get; set; }
    /// <summary>
    /// Health status of the request
    /// </summary>
    public HealthStatus HealthStatus { get; set; } = new ();
}