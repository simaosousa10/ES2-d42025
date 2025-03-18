using System.ComponentModel.DataAnnotations;

namespace ESIID42025.Models;
public class Image
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UrlImage { get; set; }  // URL da imagem

    // Chave estrangeira para Produto
    public int ProductId { get; set; }
    public Product Product { get; set; }
}