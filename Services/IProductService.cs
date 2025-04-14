using ESIID42025.Models;

namespace ESIID42025.Services;

public interface IProductService
{
    Task<List<Product>> GetAllProductsAsync();
    Task<List<ProductService.ProductWithPriceDto>> GetAllProductsWithCurrentPriceAsync();
    Task<Product> GetProductByIdAsync(int id);
    Task AddProductAsync(Product product);
    Task UpdateProductAsync(Product product, int id);
    Task DeleteProductAsync(int id);
    Task AddStoreToProductAsync(int productId, StoreProd storeProduct);
    Task AddImageToProductAsync(int productId, Image image);
    Task AddPriceToProductAsync(int productId, Price price);
    Task<Product> GetProductWithStoresAndImagesAsync(int id);
}