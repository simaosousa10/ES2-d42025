using ESIID42025.Models;

namespace ESIID42025.Services.Strategies;

public class ConfirmationStrategy : ICredibilityStrategy
{
    public double Calculate(Price price)
    {
        int confirmations = price.PriceConfirmations?.Count ?? 0;
        return 25 + (confirmations * 20); // +20% por confirmação
    }
}