using ESIID42025.Data;
using ESIID42025.DTOs;
using ESIID42025.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ESIID42025.Services;

public class ListUserService : IListUserService
{
    private readonly UserManager<User> _userManager;

    public ListUserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<ListUserDto>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();

        return users.Select(user => new ListUserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IsActive = user.IsActive

        }).ToList();
    }
    
    public async Task SetUserActiveStatusAsync(string userId, bool isActive)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            user.IsActive = isActive;
            await _userManager.UpdateAsync(user);
        }
    }


}