using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD_PROJEKT.database;
using APBD_PROJEKT.model;
using APBD_PROJEKT.Service;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace APBD_PROJEKT.Tests.Service;

public class SubscriptionServiceTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: "SubscriptionServiceTest")
        .Options;

    [Fact]
    public async Task GetAllSubscriptionsAsync_ReturnsAllSubscriptions()
    {
        await using (var context = new ApplicationDbContext(_options))
        {
            context.Subscriptions.AddRange(GetTestSubscriptions());
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationDbContext(_options))
        {
            var service = new SubscriptionService(context);
            var result = await service.GetAllSubscriptionsAsync();
            Assert.Equal(2, result.Count());
        }
    }

    [Fact]
    public async Task GetSubscriptionByIdAsync_ReturnsSubscription()
    {
        await using (var context = new ApplicationDbContext(_options))
        {
            context.Subscriptions.AddRange(GetTestSubscriptions());
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationDbContext(_options))
        {
            var service = new SubscriptionService(context);
            var result = await service.GetSubscriptionByIdAsync(1);
            Assert.Equal(1, result.Id);
        }
    }

    [Fact]
    public async Task CreateSubscriptionAsync_AddsSubscription()
    {
        await using (var context = new ApplicationDbContext(_options))
        {
            context.Clients.AddRange(GetTestClients());
            context.Softwares.AddRange(GetTestSoftware());
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationDbContext(_options))
        {
            var service = new SubscriptionService(context);
            var newSubscription = new Subscription
            {
                Id = 3, ClientId = 1, SoftwareId = 1, RenewalPeriod = "monthly", Price = 100, StartDate = DateTime.Now
            };
            var result = await service.CreateSubscriptionAsync(newSubscription);
            Assert.Equal(100, result.Price);
            Assert.Equal(1, result.ClientId);
            Assert.Equal(1, result.SoftwareId);
            Assert.Equal(3, context.Subscriptions.Count());
        }
    }

    [Fact]
    public async Task UpdateSubscriptionAsync_UpdatesSubscription()
    {
        await using (var context = new ApplicationDbContext(_options))
        {
            context.Subscriptions.AddRange(GetTestSubscriptions());
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationDbContext(_options))
        {
            var service = new SubscriptionService(context);
            var existingSubscription = context.Subscriptions.First();
            existingSubscription.Price = 150;
            var result = await service.UpdateSubscriptionAsync(existingSubscription.Id, existingSubscription);
            Assert.True(result);
            Assert.Equal(150, context.Subscriptions.First().Price);
        }
    }

    [Fact]
    public async Task DeleteSubscriptionAsync_DeletesSubscription()
    {
        await using (var context = new ApplicationDbContext(_options))
        {
            context.Subscriptions.AddRange(GetTestSubscriptions());
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationDbContext(_options))
        {
            var service = new SubscriptionService(context);
            var result = await service.DeleteSubscriptionAsync(1);
            Assert.True(result);
            Assert.False(context.Subscriptions.First().IsActive);
        }
    }

    private List<Subscription> GetTestSubscriptions()
    {
        return
        [
            new Subscription
            {
                Id = 1, ClientId = 1, SoftwareId = 1, RenewalPeriod = "monthly", Price = 100, StartDate = DateTime.Now,
                IsActive = true
            },

            new Subscription
            {
                Id = 2, ClientId = 2, SoftwareId = 2, RenewalPeriod = "yearly", Price = 1200, StartDate = DateTime.Now,
                IsActive = true
            }
        ];
    }

    private List<Client> GetTestClients()
    {
        return
        [
            new IndividualClient { Id = 1, FirstName = "John", LastName = "Doe", PESEL = "12345678901" },
            new CompanyClient { Id = 2, CompanyName = "Test Company", KRS = "0000123456" }
        ];
    }

    private List<Software> GetTestSoftware()
    {
        return
        [
            new Software
            {
                Id = 1, Name = "Test Software 1", Description = "Description 1", CurrentVersion = "1.0",
                Category = "Category 1"
            },

            new Software
            {
                Id = 2, Name = "Test Software 2", Description = "Description 2", CurrentVersion = "2.0",
                Category = "Category 2"
            }
        ];
    }
}