using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ESIID42025.Models;

public class User : IdentityUser
{
    public string Name { get; set; }
    public DateTime RegisterDate { get; set; }
    
    // Navigation Properties
    public ICollection<Message> Messages { get; set; }
    public ICollection<PriceConfirmation> PriceConfirmations { get; set; }
    public ICollection<Report> Reports { get; set; }
    public ICollection<Price> Prices { get; set; }
}