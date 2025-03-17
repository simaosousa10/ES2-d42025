using ESIID42025.Data;
using ESIID42025.Models;

namespace ESIID42025.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]

public class StoreProdController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public StoreProdController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StoreProd>>> GetStoreProducts()
    {
        return await _context.StoreProducts.ToListAsync();
    }

    [HttpGet("{storeId}/{productId}")]
    public async Task<ActionResult<StoreProd>> GetStoreProd(int storeId, int productId)
    {
        var storeProd = await _context.StoreProducts.FindAsync(storeId, productId);
        if (storeProd == null)
        {
            return NotFound();
        }
        return storeProd;
    }

    [HttpPost]
    public async Task<ActionResult<StoreProd>> PostStoreProd(StoreProd storeProd)
    {
        _context.StoreProducts.Add(storeProd);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetStoreProd), new { storeId = storeProd.StoreID, productId = storeProd.ProductID }, storeProd);
    }

    [HttpPut("{storeId}/{productId}")]
    public async Task<IActionResult> PutStoreProd(int storeId, int productId, StoreProd storeProd)
    {
        if (storeId != storeProd.StoreID || productId != storeProd.ProductID)
        {
            return BadRequest();
        }

        _context.Entry(storeProd).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{storeId}/{productId}")]
    public async Task<IActionResult> DeleteStoreProd(int storeId, int productId)
    {
        var storeProd = await _context.StoreProducts.FindAsync(storeId, productId);
        if (storeProd == null)
        {
            return NotFound();
        }

        _context.StoreProducts.Remove(storeProd);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}