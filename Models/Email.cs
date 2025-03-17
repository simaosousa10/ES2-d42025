using System.ComponentModel.DataAnnotations;

namespace ESIID42025.Models;

public class Email
{
    [Key]
    public int ID { get; set; }
    
    public string User_Email { get; set; }

    // Foreign Key
    public int UserID { get; set; }
    public User User { get; set; }
}