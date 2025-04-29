using ESIID42025.Models;

namespace ESIID42025.Services.Strategies;

public class TimeDecayStrategy : ICredibilityStrategy
{
    public double Calculate(Price price)
    {
        double daysOld = (DateTime.UtcNow - price.Date).TotalDays;
        return Math.Max(25 - (daysOld * 1), 0); // -1% por dia (mínimo 0)
    }
}