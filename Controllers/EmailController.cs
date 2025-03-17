using ESIID42025.Data;
using ESIID42025.Models;

namespace ESIID42025.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]

public class EmailController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public EmailController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Email>>> GetEmails()
    {
        return await _context.Emails.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Email>> GetEmail(int id)
    {
        var email = await _context.Emails.FindAsync(id);
        if (email == null)
        {
            return NotFound();
        }
        return email;
    }

    [HttpPost]
    public async Task<ActionResult<Email>> PostEmail(Email email)
    {
        _context.Emails.Add(email);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEmail), new { id = email.ID }, email);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutEmail(int id, Email email)
    {
        if (id != email.ID)
        {
            return BadRequest();
        }
        _context.Entry(email).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmail(int id)
    {
        var email = await _context.Emails.FindAsync(id);
        if (email == null)
        {
            return NotFound();
        }
        _context.Emails.Remove(email);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}