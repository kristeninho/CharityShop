using AutoMapper;
using CharityShop.Contracts.Dtos;
using CharityShop.Data.Repositories;
using CharityShop.Exceptions;
using Microsoft.EntityFrameworkCore;

using Product = CharityShop.Data.Models.Product;

namespace CharityShop.Services;
/// <summary>
/// Service for <see cref="Product"/> related logic
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Get all existing <see cref="Product"/>-s with their stock information
    /// </summary>
    /// <returns><see cref="MultipleProductsResult"/> with <see cref="HealthStatus"/> and List of <see cref="ProductResponseDto"/>-s of all <see cref="Product"/>-s and their stocks</returns>
    Task<MultipleProductsResult> GetAllProductsAsync();
    /// <summary>
    /// Release stock of given <see cref="Product"/>-s and return their updated stocks
    /// </summary>
    /// <param name="productsRequestDtos">List of <see cref="ProductRequestDto"/>-s with <see cref="Product"/> info for which stock is to be released</param>
    /// <returns><see cref="MultipleProductsResult"/> with <see cref="HealthStatus"/> and List of <see cref="ProductResponseDto"/>-s of updated <see cref="Product"/>-s with their up-to-date stocks</returns>
    Task<MultipleProductsResult> ReleaseProductsAsync(List<ProductRequestDto> productsRequestDtos);
    /// <summary>
    /// Book single <see cref="Product"/> and return its updated stock
    /// </summary>
    /// <param name="productId">Id of <see cref="Product"/> to be booked</param>
    /// <returns><see cref="SingleProductResult"/> with <see cref="HealthStatus"/> and <see cref="ProductResponseDto"/> of updated <see cref="Product"/> stock</returns>
    Task<SingleProductResult> BookProductAsync(int productId);
    /// <summary>
    /// Finalize the stock of <see cref="Product"/>-s and return their updated stocks. This means turn BookedQuantity into TotalQuantity.
    /// </summary>
    /// <param name="productsRequestDtos">List of <see cref="ProductRequestDto"/>-s with <see cref="Product"/> info for which stock is to be finalized</param>
    /// <returns><see cref="MultipleProductsResult"/> with <see cref="HealthStatus"/> and List of <see cref="ProductResponseDto"/>-s of updated <see cref="Product"/>-s with their up-to-date stocks</returns>
    Task<MultipleProductsResult> FinalizeProductsAsync(List<ProductRequestDto> productsRequestDtos);
    /// <summary>
    /// Initialize the stock of <see cref="Product"/>-s of type <see cref="Common.Product.ProductType.Items"/>
    /// </summary>
    /// <param name="productsRequestDtos">List of <see cref="ProductRequestDto"/>-s with quantities to be added</param>
    Task InitializeItemsStockAsync(List<ProductRequestDto> productsRequestDtos);
}

/// <inheritdoc/>
public class ProductService(IProductRepository productRepository, IMapper mapper) : IProductService
{
    private const int MaxRetryCount = 10;
    
    /// <inheritdoc/>
    public async Task<MultipleProductsResult> GetAllProductsAsync()
    {
        var productResponseDtos = await mapper.ProjectTo<ProductResponseDto>(productRepository.GetAllProductsAsQueryable())
            .ToListAsync();
        
        return new MultipleProductsResult(){Products = productResponseDtos};
    }

    /// <inheritdoc/>
    public async Task<MultipleProductsResult> ReleaseProductsAsync(List<ProductRequestDto> productsRequestDtos)
    {
        ValidateProductRequestDtos(productsRequestDtos);
        
        var retryCount = 0;
        while (true)
        {
            var products = await GetProductsForUpdateAsQueryable(productsRequestDtos).ToListAsync();
            try
            {
                foreach (var product in products)
                {
                    var dto = productsRequestDtos.First(p => p.Id == product.Id);
                    if (product.BookedQuantity < dto.Quantity)
                        throw new CustomException("Quantity to be released can not exceed BookedQuantity");

                    product.BookedQuantity -= dto.Quantity;
                }

                await productRepository.SaveChanges();

                var productResponseDtos = mapper.Map<List<ProductResponseDto>>(products);

                return new MultipleProductsResult() { Products = productResponseDtos };
            }
            catch (DbUpdateConcurrencyException) // Another instance has concurrently modified the same Product in database
            {
                retryCount++;

                if (retryCount >= MaxRetryCount)
                {
                    throw new Exception("Failed to update product after multiple retries. Please try again later.");
                }
            }
            catch (CustomException ex) // Problem regards the availability of the Product has occured
            {
                var productResponseDtos = mapper.Map<List<ProductResponseDto>>(products);
        
                return new MultipleProductsResult(){Products = productResponseDtos, HealthStatus = new HealthStatus(ex.Message)};
            }
        }
    }
    
    /// <inheritdoc/>
    public async Task<SingleProductResult> BookProductAsync(int productId)
    {
        var retryCount = 0;
        while (true)
        {
            var product = await productRepository.GetProductById(productId);
            if (product == null) throw new Exception("Product not found");
            
            try
            {
                if (product.TotalQuantity == 0) throw new CustomException($"{product} stock is empty.");
                if (product.BookedQuantity >= product.TotalQuantity) throw new CustomException($"{product} stock is fully booked.");

                product.BookedQuantity++;
                
                await productRepository.SaveChanges();

                var productResponseDto = mapper.Map<ProductResponseDto>(product);

                return new SingleProductResult(){Product = productResponseDto};
            }
            catch (DbUpdateConcurrencyException) // Another instance has concurrently modified the same Product in database
            {
                retryCount++;

                if (retryCount >= MaxRetryCount)
                {
                    throw new Exception("Failed to update product after multiple retries. Please try again later.");
                }
            }
            catch (CustomException ex) // Problem regards the availability of the Product has occured
            {
                var productResponseDto = mapper.Map<ProductResponseDto>(product);

                return new SingleProductResult(){Product = productResponseDto, HealthStatus = new HealthStatus(ex.Message)};
            }
        }
    }

    /// <inheritdoc/>
    public async Task<MultipleProductsResult> FinalizeProductsAsync(List<ProductRequestDto> productsRequestDtos)
    {
        ValidateProductRequestDtos(productsRequestDtos);
        
        var retryCount = 0;
        while (true)
        {
            var products = await GetProductsForUpdateAsQueryable(productsRequestDtos).ToListAsync();
            try
            {
                foreach (var product in products)
                {
                    var dto = productsRequestDtos.First(p => p.Id == product.Id);
                    
                    if (product.TotalQuantity < dto.Quantity)
                        throw new CustomException($"{product.Name} stock has changed and the purchase can not be completed. Maximum amount of {product.Name} have been reserved.");

                    product.BookedQuantity -= dto.Quantity;
                    product.TotalQuantity -= dto.Quantity;
                }

                await productRepository.SaveChanges();

                var productResponseDtos = mapper.Map<List<ProductResponseDto>>(products);
        
                return new MultipleProductsResult(){Products = productResponseDtos};
            }
            catch (DbUpdateConcurrencyException) // Another instance has concurrently modified the same Product in database 
            {
                retryCount++;

                if (retryCount >= MaxRetryCount)
                {
                    throw new Exception("Failed to update product after multiple retries. Please try again later.");
                }
            }
            catch (CustomException ex) // Problem regards the availability of the Product has occured
            {
                var productResponseDtos = mapper.Map<List<ProductResponseDto>>(products);
        
                return new MultipleProductsResult(){Products = productResponseDtos, HealthStatus = new HealthStatus(ex.Message)};
            }
        }
    }

    /// <inheritdoc/>
    public async Task InitializeItemsStockAsync(List<ProductRequestDto> productsRequestDtos)
    {
        // TODO: exception middleware ?
        ValidateProductRequestDtos(productsRequestDtos);

        var products = await GetProductsForUpdateAsQueryable(productsRequestDtos).ToListAsync();

        if (products.Any(p => p.Type != Common.Product.ProductType.Items)) 
            throw new Exception($"Only products of type {nameof(Common.Product.ProductType.Items)} quantities can be initialized");

        foreach (var product in products)
        {
            product.TotalQuantity = productsRequestDtos.First(p => p.Id == product.Id).Quantity;
        }

        await productRepository.SaveChanges();
    }

    /// <summary>
    /// Get <see cref="Product"/>-s for update as <see cref="IQueryable{T}"/> and .AsTracking()
    /// </summary>
    /// <param name="productsRequestDtos">List of <see cref="ProductRequestDto"/>-s with <see cref="Product"/> Id-s which are to be updated</param>
    /// <returns><see cref="IQueryable"/> of <see cref="Product"/> with Tracking</returns>
    private IQueryable<Product> GetProductsForUpdateAsQueryable(List<ProductRequestDto> productsRequestDtos)
    {
        var productIds = productsRequestDtos.Select(x => x.Id);
        return productRepository.GetAllProductsAsQueryable().Where(x => productIds.Contains(x.Id)).AsTracking();
    }

    /// <summary>
    /// Validate List of <see cref="ProductRequestDto"/>-s is not null or empty and all Quantities are valid
    /// </summary>
    /// <param name="productsRequestDtos">List of <see cref="ProductRequestDto"/>-s to be validated</param>
    /// <exception cref="ArgumentNullException">List of <see cref="ProductRequestDto"/> is null or empty</exception>
    /// <exception cref="Exception">Any of given Quantities is invalid</exception>
    private static void ValidateProductRequestDtos(List<ProductRequestDto> productsRequestDtos)
    {
        if (productsRequestDtos == null || !productsRequestDtos.Any()) throw new ArgumentNullException(nameof(productsRequestDtos));
        if (productsRequestDtos.Any(p => p.Quantity < 0)) throw new Exception($"{nameof(ProductRequestDto.Quantity)} can not be negative");
    }
}
