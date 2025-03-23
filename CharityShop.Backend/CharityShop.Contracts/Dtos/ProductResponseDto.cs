namespace CharityShop.Contracts.Dtos;

/// <summary>
/// Represents a Product
/// </summary>
public class ProductResponseDto
{
    /// <summary>
    /// Id of the Product
    /// </summary>
    public int Id { get; init; }
    /// <summary>
    /// Name of the Product
    /// </summary>
    public string Name { get; init; }
    /// <summary>
    /// Price of the Product
    /// </summary>
    public double Price { get; init; }
    /// <summary>
    /// Type of the Product
    /// </summary>
    public string Type { get; init; }
    /// <summary>
    /// Total quantity of Product left
    /// </summary>
    public int TotalQuantity { get; init; }
    /// <summary>
    /// Booked quantity of Product
    /// </summary>
    public int BookedQuantity { get; init; }
    /// <summary>
    /// Available quantity of Product
    /// </summary>
    public int AvailableQuantity => TotalQuantity - BookedQuantity;
}