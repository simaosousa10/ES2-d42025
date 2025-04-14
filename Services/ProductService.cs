using ESIID42025.Data;
using ESIID42025.Models;
using Microsoft.EntityFrameworkCore;

namespace ESIID42025.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Product>> GetAllProductsAsync()
    {
        var result = await _context.Products
            .Include(p => p.Prices)
            .Include(p => p.StoreProducts)
            .Include(p => p.Images)
            .ToListAsync();
        return result;
    }
    
    public async Task<List<ProductWithPriceDto>> GetAllProductsWithCurrentPriceAsync()
    {
        var products = await _context.Products
            .Include(p => p.Prices)
            .Include(p => p.Category)
            .ToListAsync();
        
        return products.Select(p => new ProductWithPriceDto
        {
            Id = p.ID,
            Name = p.Name,
            Description = p.Description,
            RegistrationDate = p.Registration_Date,
            CurrentPrice = p.Prices.OrderByDescending(pr => pr.Date).FirstOrDefault()?.Value,
            CategoryName = p.Category?.Name
        }).ToList();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Prices)
            .Include(p => p.StoreProducts)
            .Include(p => p.Images)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.ID == id);
    }

    public async Task AddProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(Product product, int id)
    {
        var DBproduct = await _context.Products.FindAsync(id);
        if (DBproduct != null)
        {
            DBproduct.Name = product.Name;
            DBproduct.Description = product.Description;
            DBproduct.Registration_Date = product.Registration_Date;
            DBproduct.Prices = product.Prices;
            DBproduct.StoreProducts = product.StoreProducts;
            DBproduct.Images = product.Images;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task AddStoreToProductAsync(int productId, StoreProd storeProduct)
    {
        var product = await _context.Products
            .Include(p => p.StoreProducts)
            .FirstOrDefaultAsync(p => p.ID == productId);

        if (product == null)
            throw new KeyNotFoundException($"Product with id {productId} not found");

        // Configura o relacionamento
        storeProduct.ProductID = productId;
        
        // Se a Store já existe, atualiza em vez de adicionar
        var existingStoreProduct = product.StoreProducts
            .FirstOrDefault(sp => sp.StoreID == storeProduct.StoreID);
        
        if (existingStoreProduct != null)
        {
            // Atualiza os dados da loja existente
            _context.Entry(existingStoreProduct).CurrentValues.SetValues(storeProduct);
        }
        else
        {
            // Adiciona uma nova loja ao produto
            product.StoreProducts.Add(storeProduct);
        }

        await _context.SaveChangesAsync();
    }

    public async Task AddImageToProductAsync(int productId, Image image)
    {
        var product = await _context.Products
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.ID == productId);

        if (product == null)
            throw new KeyNotFoundException($"Product with id {productId} not found");

        // Configura o relacionamento
        image.ProductId = productId;
        product.Images.Add(image);

        await _context.SaveChangesAsync();
    }

    public async Task AddPriceToProductAsync(int productId, Price price)
    {
        var product = await _context.Products
            .Include(p => p.Prices)
            .FirstOrDefaultAsync(p => p.ID == productId);

        if (product == null)
            throw new KeyNotFoundException($"Product with id {productId} not found");

        // Configura o relacionamento e data
        price.ID = productId;
        price.Date = DateTime.UtcNow; // Garante a data atual
        
        product.Prices.Add(price);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Product> GetProductWithStoresAndImagesAsync(int id)
    {
        return await _context.Products
                   .Include(p => p.StoreProducts)
                   .ThenInclude(sp => sp.Store)
                   .Include(p => p.Images)
                   .Include(p => p.Prices)
                   .ThenInclude(p => p.Store)
                   .Include(p => p.Category) // Se precisar da categoria
                   .FirstOrDefaultAsync(p => p.ID == id) 
               ?? throw new KeyNotFoundException($"Product with id {id} not found");
    }
    
    public class ProductWithPriceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime RegistrationDate { get; set; }
        public double? CurrentPrice { get; set; }
        public string CategoryName { get; set; }
    }
    
}