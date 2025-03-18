using System.ComponentModel.DataAnnotations;

namespace ESIID42025.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    public string Username { get; set; }
    public string Email { get; set; } 
    public string Name { get; set; }
    public string Password { get; set; }
    public DateTime RegisterDate { get; set; }
    
    // Navigation Properties
    public ICollection<Message> Messages { get; set; }
    public ICollection<PriceConfirmation> PriceConfirmations { get; set; }
    public ICollection<Report> Reports { get; set; }
    public ICollection<Price> Prices { get; set; }
}