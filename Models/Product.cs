using System.ComponentModel.DataAnnotations;

namespace ESIID42025.Models;

public class Product
{
    [Key]
    public int ID { get; set; }
    
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Registration_Date { get; set; }

    // Foreign Keys
    public int? ReportID { get; set; }
    public Report Report { get; set; }

    public int CategoryID { get; set; }
    public Category Category { get; set; }

    // Navigation Property
    public ICollection<Price> Prices { get; set; }
    public ICollection<StoreProd> StoreProducts { get; set; }
    public ICollection<Image> Images { get; set; }
}