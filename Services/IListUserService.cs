using ESIID42025.DTOs;

namespace ESIID42025.Services;

public interface IListUserService
{
    Task<List<ListUserDto>> GetAllUsersAsync();
    
    Task SetUserActiveStatusAsync(string userId, bool isActive);

}