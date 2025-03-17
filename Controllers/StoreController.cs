﻿using ESIID42025.Data;
using ESIID42025.Models;

namespace ESIID42025.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]

public class StoreController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public StoreController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Store>>> GetStores()
    {
        return await _context.Stores.ToListAsync();
    }

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

    [HttpPost]
    public async Task<ActionResult<Store>> PostStore(Store store)
    {
        _context.Stores.Add(store);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetStore), new { id = store.ID }, store);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutStore(int id, Store store)
    {
        if (id != store.ID)
        {
            return BadRequest();
        }
        _context.Entry(store).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

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