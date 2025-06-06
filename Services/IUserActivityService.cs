
using System.Threading.Tasks;
using ESIID42025.Components.Account.Pages;

public interface IUserActivityService
{
    Task<UserActivities.UserActivitySummary> GetActivitySummaryAsync(string userId);
}
