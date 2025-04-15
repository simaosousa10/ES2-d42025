using ESIID42025.Models;

namespace ESIID42025.Services;

public interface IPriceService
{
    Task AddPriceAsync(int productId, Price price);
}
