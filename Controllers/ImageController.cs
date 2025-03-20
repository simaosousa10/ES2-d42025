using ESIID42025.Data;
using ESIID42025.Models;

namespace ESIID42025.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]

public class ImageController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ImageController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Image>>> GetImages()
    {
        return await _context.Images.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Image>> GetImage(int id)
    {
        var image = await _context.Images.FindAsync(id);
        if (image == null)
        {
            return NotFound();
        }
        return image;
    }

    [HttpPost]
    public async Task<ActionResult<Image>> PostImage(Image image)
    {
        _context.Images.Add(image);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetImage), new { id = image.Id }, image);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutImage(int id, Image image)
    {
        if (id != image.Id)
        {
            return BadRequest();
        }

        _context.Entry(image).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteImage(int id)
    {
        var image = await _context.Images.FindAsync(id);
        if (image == null)
        {
            return NotFound();
        }

        _context.Images.Remove(image);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}