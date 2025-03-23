namespace CharityShop.WebAPI;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int TotalQuantity { get; set; }
    public int BookedQuantity { get; set; }

    public int AvailableQuantity => TotalQuantity - BookedQuantity;
}