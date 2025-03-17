using ESIID42025.Data;
using ESIID42025.Models;

namespace ESIID42025.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]

public class PriceConfirmationController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public PriceConfirmationController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PriceConfirmation>>> GetPriceConfirmations()
    {
        return await _context.PriceConfirmations.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PriceConfirmation>> GetPriceConfirmation(int id)
    {
        var priceConfirmation = await _context.PriceConfirmations.FindAsync(id);
        if (priceConfirmation == null)
        {
            return NotFound();
        }
        return priceConfirmation;
    }

    [HttpPost]
    public async Task<ActionResult<PriceConfirmation>> PostPriceConfirmation(PriceConfirmation priceConfirmation)
    {
        _context.PriceConfirmations.Add(priceConfirmation);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPriceConfirmation), new { id = priceConfirmation.ID }, priceConfirmation);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPriceConfirmation(int id, PriceConfirmation priceConfirmation)
    {
        if (id != priceConfirmation.ID)
        {
            return BadRequest();
        }
        _context.Entry(priceConfirmation).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePriceConfirmation(int id)
    {
        var priceConfirmation = await _context.PriceConfirmations.FindAsync(id);
        if (priceConfirmation == null)
        {
            return NotFound();
        }
        _context.PriceConfirmations.Remove(priceConfirmation);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}