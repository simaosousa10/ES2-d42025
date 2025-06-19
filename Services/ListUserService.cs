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

        var userDtos = new List<ListUserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";

            userDtos.Add(new ListUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                Role = role
            });
        }

        return userDtos;
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
    
    public async Task UpdateUserRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return;

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, role);
    }

}