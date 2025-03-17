using System.ComponentModel.DataAnnotations;

namespace ESIID42025.Models;

public class Store
{
    [Key]
    public int ID { get; set; }
    
    public string Name { get; set; }
    public string Location { get; set; }
    public string GoogleMaps_URL { get; set; }
    public DateTime Registration_Date { get; set; }

    // Foreign Key
    public int ReportID { get; set; }
    public Report Report { get; set; }

    // Navigation Property
    public ICollection<Price> Prices { get; set; }
}