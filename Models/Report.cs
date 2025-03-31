using System.ComponentModel.DataAnnotations;

namespace ESIID42025.Models;

public class Report
{
    [Key]
    public int ID { get; set; }
    
    public string Type { get; set; }
    public string Content { get; set; }
    public DateTime Sent_Date { get; set; }

    // Foreign Key
    public string UserID { get; set; }
    public User User { get; set; }
    
    // Navigation Property
    public ICollection<Store> Stores { get; set; }
    public ICollection<Product> Products { get; set; }
}