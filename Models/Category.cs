using System.ComponentModel.DataAnnotations;

namespace ESIID42025.Models;

public class Category
{
    [Key]
    public int ID { get; set; }
    
    public string Name { get; set; }
    public string Description { get; set; }

    // Navigation Property
    public ICollection<Product> Products { get; set; }
}