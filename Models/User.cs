using Microsoft.AspNetCore.Identity;

namespace ESIID42025.Models;

// Models/User.cs
public class User : IdentityUser
{
    public string? Name { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? DateofBirth { get; set; }

    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<Report> Reports { get; set; } = new List<Report>();

    public ICollection<Price> Prices { get; set; } = new List<Price>();
    public ICollection<PriceConfirmation> PriceConfirmations { get; set; } = new List<PriceConfirmation>();
}
