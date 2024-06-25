using APBD_PROJEKT.database;
using APBD_PROJEKT.model;

namespace APBD_PROJEKT.Service;

public class RevenueService(ApplicationDbContext context)
{
    public Task<RevenueCalculation> CalculateRevenue()
    {
        var currentRevenue = context.Contracts.Where(c => c.IsSigned).Sum(c => c.Price);
        var predictedRevenue = context.Contracts.Where(c => !c.IsSigned).Sum(c => c.Price) + currentRevenue;

        var activeSubscriptions = context.Subscriptions.Where(s => s.IsActive);
        var subscriptionRevenue = activeSubscriptions.Sum(s => s.Price * (s.RenewalPeriod == "monthly" ? 12 : 1));

        predictedRevenue += subscriptionRevenue;

        return Task.FromResult(new RevenueCalculation
        {
            CurrentRevenue = currentRevenue,
            PredictedRevenue = predictedRevenue
        });
    }
}