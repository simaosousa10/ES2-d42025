
using System.Threading.Tasks;
using ESIID42025.Data;
using ESIID42025.Components.Account.Pages;
using Microsoft.EntityFrameworkCore;

public class UserActivityService : IUserActivityService
{
    private readonly ApplicationDbContext _context;

    public UserActivityService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserActivities.UserActivitySummary> GetActivitySummaryAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        var totalPrices = await _context.Prices.CountAsync(p => p.UserID == userId);
        var totalConfirmations = await _context.PriceConfirmations.CountAsync(p => p.UserID == userId);
        var totalMessages = await _context.Messages.CountAsync(m => m.UserID == userId);
        var totalReports = await _context.Reports.CountAsync(r => r.UserID == userId);

        var summary = new UserActivities.UserActivitySummary
        {
            Nome = user?.Name ?? "Utilizador",
            TotalPrices = totalPrices,
            TotalConfirmations = totalConfirmations,
            TotalMessages = totalMessages,
            TotalReports = totalReports
        };
        
        return summary;
    }

}
