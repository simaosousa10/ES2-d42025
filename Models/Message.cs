using System.ComponentModel.DataAnnotations;

namespace ESIID42025.Models;

public class Message
{
    [Key]
    public int ID { get; set; }
    
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime Sent_Date { get; set; }
    
    // Foreign Key
    public string UserID { get; set; }
    public User User { get; set; }
}