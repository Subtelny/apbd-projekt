using System;
using System.Threading.Tasks;
using APBD_PROJEKT.database;
using APBD_PROJEKT.model;
using APBD_PROJEKT.Service;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace APBD_PROJEKT.Tests.Service;

[TestSubject(typeof(RevenueService))]
public class RevenueServiceTests
{
    [Fact]
    public async Task CalculateRevenue_ReturnsCorrectRevenue()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "RevenueRecognitionTest")
            .Options;

        await using (var context = new ApplicationDbContext(options))
        {
            context.Contracts.Add(new Contract { Id = 1, Price = 1000, IsPaid = true });
            context.Contracts.Add(new Contract { Id = 2, Price = 2000, IsPaid = false });
            context.Subscriptions.Add(new Subscription
                { Id = 1, Price = 100, RenewalPeriod = "monthly", IsActive = true, StartDate = DateTime.Now });
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationDbContext(options))
        {
            var service = new RevenueService(context);
            var result = await service.CalculateRevenue();

            Assert.Equal(1000, result.CurrentRevenue);
            Assert.Equal(1000 + 2000 + (100 * 12), result.PredictedRevenue);
        }
    }
}