using ESIID42025.Models;

namespace ESIID42025.Services.Strategies;

public interface ICredibilityStrategy
{
    double Calculate(Price price);
}