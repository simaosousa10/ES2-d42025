using ESIID42025.Data;
using ESIID42025.Models;
using Microsoft.EntityFrameworkCore;

namespace ESIID42025.Services;

public class PriceService : IPriceService
{
    private readonly ApplicationDbContext _context;

    public PriceService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddPriceAsync(int productId, Price price)
    {
        var product = await _context.Products
            .Include(p => p.Prices)
            .FirstOrDefaultAsync(p => p.ID == productId);

        if (product == null)
            throw new KeyNotFoundException($"Product with id {productId} not found");

        price.ID = productId;
        price.Date = DateTime.UtcNow; // Garante a data atual
        product.Prices.Add(price);
        await _context.SaveChangesAsync();
    }
    public async Task<Price?> GetByIdAsync(int id)
    {
        return await _context.Prices.FindAsync(id);
    }

    public async Task<Price> CreateAsync(Price price)
    {
        _context.Prices.Add(price);
        await _context.SaveChangesAsync();
        return price;
    }

    public async Task<bool> UpdateAsync(int id, Price price)
    {
        if (id != price.ID) return false;

        _context.Entry(price).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var price = await _context.Prices.FindAsync(id);
        if (price == null) return false;

        _context.Prices.Remove(price);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<List<Price>> GetAllAsync()
    {
        return await _context.Prices.ToListAsync();
    }

}

