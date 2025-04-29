using System.ComponentModel.DataAnnotations;

namespace ESIID42025.Models;

public class Price
{
    [Key]
    public int ID { get; set; }
    
    public double Value { get; set; }
    public DateTime Date { get; set; }

    public double TrustScore { get; set; }
    // Foreign Keys
    public int StoreID { get; set; }
    public Store Store { get; set; }

    public int ProductID { get; set; }
    public Product Product { get; set; }

    public string UserID { get; set; }
    public User User { get; set; }
    
    // Navigation Property
    public ICollection<PriceConfirmation> PriceConfirmations { get; set; }
}