using ESIID42025.Data;
using ESIID42025.Models;

namespace ESIID42025.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]

public class PriceController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public PriceController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Price>>> GetPrices()
    {
        return await _context.Prices.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Price>> GetPrice(int id)
    {
        var price = await _context.Prices.FindAsync(id);
        if (price == null)
        {
            return NotFound();
        }
        return price;
    }

    [HttpPost]
    public async Task<ActionResult<Price>> PostPrice(Price price)
    {
        _context.Prices.Add(price);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPrice), new { id = price.ID }, price);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPrice(int id, Price price)
    {
        if (id != price.ID)
        {
            return BadRequest();
        }
        _context.Entry(price).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrice(int id)
    {
        var price = await _context.Prices.FindAsync(id);
        if (price == null)
        {
            return NotFound();
        }
        _context.Prices.Remove(price);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}