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
    public int UserID { get; set; }
    public User User { get; set; }
}