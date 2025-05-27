using ESIID42025.Data;
using ESIID42025.DTOs;
using ESIID42025.Models;
using ESIID42025.Services.Strategies;
using Microsoft.EntityFrameworkCore;

namespace ESIID42025.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    private readonly ICredibilityStrategy _hybridStrategy;
    public ProductService(ApplicationDbContext context, ICredibilityStrategy hybridStrategy)
    {
        _context = context;
        _hybridStrategy = hybridStrategy;
    }
    
    public async Task<List<Product>> GetAllProductsAsync()
    {
        var result = await _context.Products
            .Include(p => p.Prices)
            .Include(p => p.StoreProducts)
            .Include(p => p.Images)
            .Include(p => p.Category)
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
        product.Registration_Date = DateTime.UtcNow;
        
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
            DBproduct.CategoryID = product.CategoryID;
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
        price.TrustScore = 0;
        price.ID = productId;
        price.Date = DateTime.UtcNow; // Garante a data atual
        product.Prices.Add(price);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Product> GetProductWithStoresAndImagesAsync(int id)
    {
        var product = await _context.Products
                   .Include(p => p.StoreProducts)
                   .ThenInclude(sp => sp.Store)
                   .Include(p => p.Images)
                   .Include(p => p.Prices)
                   .ThenInclude(p => p.PriceConfirmations)
                   .Include(p => p.Category) // Se precisar da categoria
                   .FirstOrDefaultAsync(p => p.ID == id) 
               ?? throw new KeyNotFoundException($"Product with id {id} not found");
        
        foreach (var price in product.Prices)
        {
            price.TrustScore = _hybridStrategy.Calculate(price);
        }

        return product;
    }
    
    
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<List<Category>> SearchCategoriesAsync(string name)
    {
        return await _context.Categories
            .Where(c => c.Name.Contains(name))
            .ToListAsync();
    }

    public async Task<Category> AddCategoryAsync(Category category)
    {
        category.Name = NormalizeCategoryName(category.Name);
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    private string NormalizeCategoryName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        name = name.ToLower();
        return char.ToUpper(name[0]) + name.Substring(1);
    }
    
    
    
    
    
    public async Task DeleteImageAsync(int imageId)
    {
        var image = await _context.Images.FindAsync(imageId);
        if (image != null)
        {
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddImageToProductAsync(int productId, string imageUrl)
    {
        var image = new Image
        {
            UrlImage = imageUrl,
            ProductId = productId
        };
    
        _context.Images.Add(image);
        await _context.SaveChangesAsync();
    }
    
    
    
    
    public async Task<List<Store>> GetAllStoresAsync()
    {
        return await _context.Stores.ToListAsync();
    }

    public async Task AddStoreToProductAsync(int productId, int storeId)
    {
        var existing = await _context.StoreProducts
            .FirstOrDefaultAsync(sp => sp.ProductID == productId && sp.StoreID == storeId);
    
        if (existing == null)
        {
            _context.StoreProducts.Add(new StoreProd
            {
                ProductID = productId,
                StoreID = storeId
            });
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveStoreFromProductAsync(int productId, int storeId)
    {
        var storeProd = await _context.StoreProducts
            .FirstOrDefaultAsync(sp => sp.ProductID == productId && sp.StoreID == storeId);
    
        if (storeProd != null)
        {
            _context.StoreProducts.Remove(storeProd);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Store> AddNewStoreAsync(Store store)
    {
        store.Registration_Date = DateTime.UtcNow;
        _context.Stores.Add(store);
        await _context.SaveChangesAsync();
        return store;
    }
    
    public async Task AddPriceAsync(Price price)
    {
        price.Date = DateTime.UtcNow;
        _context.Prices.Add(price);
        await _context.SaveChangesAsync();
    }

    public async Task AddPriceConfirmationAsync(int priceId, string userId)
    {
        var price = await _context.Prices
            .Include(p => p.PriceConfirmations)
            .FirstOrDefaultAsync(p => p.ID == priceId);
        if (price == null) return;
        
        var confirmation = new PriceConfirmation
        {
            PriceID = priceId,
            UserID = userId,
            Confirmation_DateTime = DateTime.UtcNow
        };
    
        _context.PriceConfirmations.Add(confirmation);
        if (price.PriceConfirmations == null)
        {
            price.TrustScore = 15;
        }
        else
        {
            price.TrustScore = _hybridStrategy.Calculate(price);
        }
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdatePriceTrustScore(int priceId)
    {
        var price = await _context.Prices
            .Include(p => p.PriceConfirmations)
            .FirstOrDefaultAsync(p => p.ID == priceId);

        if (price != null)
        {
            price.TrustScore = _hybridStrategy.Calculate(price);
        
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<List<PriceConfirmation>> GetConfirmationsForPriceAsync(int priceId)
    {
        return await _context.PriceConfirmations
            .Where(pc => pc.PriceID == priceId)
            .ToListAsync();
    }
    
    
    public async Task<List<SearchSuggestion>> GetSearchSuggestionsAsync(string term)
    {
        term = term.ToLower();

        var productResults = await _context.Products
            .Where(p => p.Name.ToLower().Contains(term))
            .Select(p => new SearchSuggestion { Text = p.Name, Type = "product", Id = p.ID})
            .Distinct()
            .Take(5)
            .ToListAsync();

        var categoryResults = await _context.Categories
            .Where(c => c.Name.ToLower().Contains(term))
            .Select(c => new SearchSuggestion { Text = c.Name, Type = "category" })
            .Distinct()
            .Take(5)
            .ToListAsync();

        var storeResults = await _context.Stores
            .Where(s => s.Name.ToLower().Contains(term))
            .Select(s => new SearchSuggestion { Text = s.Name, Type = "store" })
            .Distinct()
            .Take(5)
            .ToListAsync();

        return productResults
            .Concat(categoryResults)
            .Concat(storeResults)
            .ToList();
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