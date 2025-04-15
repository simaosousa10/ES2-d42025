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
    
    Task<List<Category>> GetAllCategoriesAsync();
    Task<List<Category>> SearchCategoriesAsync(string name);
    Task<Category> AddCategoryAsync(Category category);
    Task<Category> GetCategoryByIdAsync(int id);
    
    Task DeleteImageAsync(int imageId);
    Task AddImageToProductAsync(int productId, string imageUrl);
    
    Task<List<Store>> GetAllStoresAsync();
    Task AddStoreToProductAsync(int productId, int storeId);
    Task RemoveStoreFromProductAsync(int productId, int storeId);
    Task<Store> AddNewStoreAsync(Store store);
    
    Task AddPriceAsync(Price price);
    Task AddPriceConfirmationAsync(int priceId, string userId);
    Task<List<PriceConfirmation>> GetConfirmationsForPriceAsync(int priceId);
}