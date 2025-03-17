using ESIID42025.Data;
using ESIID42025.Models;

namespace ESIID42025.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]

public class MessageController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public MessageController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
    {
        return await _context.Messages.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Message>> GetMessage(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message == null)
        {
            return NotFound();
        }
        return message;
    }

    [HttpPost]
    public async Task<ActionResult<Message>> PostMessage(Message message)
    {
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMessage), new { id = message.ID }, message);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutMessage(int id, Message message)
    {
        if (id != message.ID)
        {
            return BadRequest();
        }
        _context.Entry(message).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message == null)
        {
            return NotFound();
        }
        _context.Messages.Remove(message);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}