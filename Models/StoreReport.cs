using ESIID42025.Data;
using ESIID42025.Models;
using Microsoft.EntityFrameworkCore;

public class StoreReport : GeneratedReport
{
    public int StoreId { get; set; }

    public override async Task<string> GerarAsync(ApplicationDbContext context)
    {
        var store = await context.Stores
            .Include(s => s.StoreProducts)
            .ThenInclude(sp => sp.Product)
            .FirstOrDefaultAsync(s => s.ID == StoreId);

        if (store == null)
            return $"Loja com ID {StoreId} não encontrada.";

        int numProdutos = store.StoreProducts?.Count ?? 0;

        return $"[Loja] {store.Name} tem {numProdutos} produtos. Gerado em {DataGeracao}.";
    }
}