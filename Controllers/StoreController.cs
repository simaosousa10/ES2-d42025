using ESIID42025.Data;
using ESIID42025.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESIID42025.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StoreController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StoreController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Store
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Store>>> GetStores()
    {
        return await _context.Stores.ToListAsync();
    }

    // GET: api/Store/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Store>> GetStore(int id)
    {
        var store = await _context.Stores.FindAsync(id);

        if (store == null)
        {
            return NotFound();
        }

        return store;
    }

    // POST: api/Store
    [HttpPost]
    public async Task<ActionResult<Store>> PostStore([FromBody] Store store)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Stores.Add(store);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStore), new { id = store.ID }, store);
    }

    // PUT: api/Store/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutStore(int id, [FromBody] Store store)
    {
        if (id != store.ID)
        {
            return BadRequest("ID in URL does not match ID in request body.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var exists = await _context.Stores.AnyAsync(s => s.ID == id);
        if (!exists)
        {
            return NotFound();
        }

        _context.Entry(store).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(500, "Could not update the store due to a concurrency issue.");
        }

        return NoContent();
    }

    // DELETE: api/Store/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStore(int id)
    {
        var store = await _context.Stores.FindAsync(id);
        if (store == null)
        {
            return NotFound();
        }

        _context.Stores.Remove(store);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
