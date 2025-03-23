using CharityShop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CharityShop.Data.Repositories;
/// <summary>
/// Repository for <see cref="Product"/> related database operations
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Get single <see cref="Product"/> with Tracking
    /// </summary>
    /// <param name="productId">Id of <see cref="Product"/> to be fetched</param>
    Task<Product> GetProductByIdAsync(int productId);
    /// <summary>
    /// Get <see cref="IQueryable"/> of all <see cref="Product"/>-s with AsNoTracking().
    /// If Tracking is required, then override it using AsTracking()
    /// </summary>
    IQueryable<Product> GetAllProductsAsQueryable();
    /// <summary>
    /// Commit all the changes (inserts, updates, and deletes) made to the database within the current context as a single transaction
    /// </summary>
    Task SaveChangesAsync();
}

public class ProductRepository(ProductDbContext dbContext) : IProductRepository
{
    public Task<Product> GetProductByIdAsync(int productId)
    {
        return dbContext.Products.FirstOrDefaultAsync(product => product.Id == productId);
    }
    
    public IQueryable<Product> GetAllProductsAsQueryable()
    {
        return dbContext.Products.AsNoTracking();
    }

    public Task SaveChangesAsync()
    {
        return dbContext.SaveChangesAsync();
    }
}