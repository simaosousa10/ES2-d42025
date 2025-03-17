using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESIID42025.Models;

public class StoreProd
{
    [Key, Column(Order = 1)]
    public int StoreID { get; set; }
    public Store Store { get; set; }

    [Key, Column(Order = 2)]
    public int ProductID { get; set; }
    public Product Product { get; set; }
}