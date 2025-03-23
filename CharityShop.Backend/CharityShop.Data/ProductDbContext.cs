using CharityShop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CharityShop.Data;

public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .Property(p => p.RowVersion)
            .IsRowVersion(); // For concurrency handling
        
        // Seed initial data
        modelBuilder.Entity<Product>().HasData(
            // Items with preset quantities
            new Product { Id = 1, Name = "Brownie", Price = 0.65, Type = Common.Product.ProductType.Edible, TotalQuantity = 48, BookedQuantity = 0 },
            new Product { Id = 2, Name = "Muffin", Price = 1.00, Type = Common.Product.ProductType.Edible, TotalQuantity = 36, BookedQuantity = 0 },
            new Product { Id = 3, Name = "Cake pop", Price = 1.35, Type = Common.Product.ProductType.Edible, TotalQuantity = 24, BookedQuantity = 0 },
            new Product { Id = 4, Name = "Apple tart", Price = 1.50, Type = Common.Product.ProductType.Edible, TotalQuantity = 60, BookedQuantity = 0 },
            new Product { Id = 5, Name = "Water", Price = 1.50, Type = Common.Product.ProductType.Edible, TotalQuantity = 30, BookedQuantity = 0 },
            
            // Items with no preset quantities
            new Product { Id = 6, Name = "Shirt", Price = 2.00, Type = Common.Product.ProductType.Items, TotalQuantity = 0, BookedQuantity = 0 },
            new Product { Id = 7, Name = "Pants", Price = 3.00, Type = Common.Product.ProductType.Items, TotalQuantity = 0, BookedQuantity = 0 },
            new Product { Id = 8, Name = "Jacket", Price = 4.00, Type = Common.Product.ProductType.Items, TotalQuantity = 0, BookedQuantity = 0 },
            new Product { Id = 9, Name = "Toy", Price = 1.00, Type = Common.Product.ProductType.Items, TotalQuantity = 0, BookedQuantity = 0 }
        );
    }
}