

using ESIID42025.Data;
using ESIID42025.Models;
using Microsoft.AspNetCore.Identity;

namespace ESIID42025.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public UserController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public ActionResult<IEnumerable<User>> GetUsers()
    {
        return _userManager.Users.ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return user;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] User userModel, string password)
    {
        var result = await _userManager.CreateAsync(userModel, password);
        if (result.Succeeded)
        {
            return Ok("Utilizador criado com sucesso!");
        }
        return BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        await _userManager.DeleteAsync(user);
        return NoContent();
    }
}
