using System.ComponentModel.DataAnnotations;

namespace ESIID42025.Models;

public class PriceConfirmation
{
    [Key]
    public int ID { get; set; }
    
    public DateTime Confirmation_DateTime { get; set; }

    // Foreign Keys
    public string UserID { get; set; }
    public User User { get; set; }

    public int PriceID { get; set; }
    public Price Price { get; set; }
}