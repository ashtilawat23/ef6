using Backend.Application.Repositories;
using Backend.Domain.Entities;

namespace Backend.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        product.CreatedAt = DateTime.UtcNow;
        return await _productRepository.AddAsync(product);
    }

    public async Task<Product?> UpdateProductAsync(int id, Product product)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null) return null;

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.StockQuantity = product.StockQuantity;
        existingProduct.UpdatedAt = DateTime.UtcNow;

        return await _productRepository.UpdateAsync(existingProduct);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return false;

        await _productRepository.DeleteAsync(product);
        return true;
    }
} 