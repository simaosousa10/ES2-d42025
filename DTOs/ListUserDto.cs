namespace ESIID42025.DTOs;

public class ListUserDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public string Role { get; set; } = "";

}