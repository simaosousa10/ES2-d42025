using ESIID42025.Models;

namespace ESIID42025.Services.Strategies;

public class HybridStrategy : ICredibilityStrategy
{
    public double Calculate(Price price)
    {
        int confirmations = price.PriceConfirmations?.Count ?? 0;
        double daysOld = (DateTime.UtcNow - price.Date).TotalDays;
        return Math.Clamp(25 + (confirmations * 20) - (daysOld * 1), 0, 100);
    }
}