using ESIID42025.Data;

namespace ESIID42025.Models;

public abstract class GeneratedReport
{
    public DateTime DataGeracao { get; set; } = DateTime.UtcNow;
    public abstract Task<string> GerarAsync(ApplicationDbContext context);
}