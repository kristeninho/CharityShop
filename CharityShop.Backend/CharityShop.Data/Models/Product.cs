namespace CharityShop.Data.Models;
/// <summary>
/// Product domain db model
/// </summary>
public class Product
{
    /// <summary>
    /// Id of Product
    /// </summary>
    public int Id { get; init; }
    /// <summary>
    /// Name of Product
    /// </summary>
    public string Name { get; init; }
    /// <summary>
    /// Price of Product
    /// </summary>
    public double Price { get; init; }
    /// <summary>
    /// Type of Product. Supported types are configured <see cref="Common.Product.ProductType"/> enum
    /// </summary>
    public Common.Product.ProductType Type { get; init; }
    /// <summary>
    /// Total quantity of Product showing how many Products are left
    /// </summary>
    public int TotalQuantity { get; set; }
    /// <summary>
    /// Booked quantity of Product showing how many Products are booked to be bought
    /// </summary>
    public int BookedQuantity { get; set; }
    /// <summary>
    /// RowVersion to handle Concurrency
    /// </summary>
    public uint RowVersion {get;init;}
}