using APBD_PROJEKT.database;
using APBD_PROJEKT.model;
using Microsoft.EntityFrameworkCore;

namespace APBD_PROJEKT.Service;

public class SubscriptionService(ApplicationDbContext context)
{
    public async Task<Subscription?> CreateSubscriptionAsync(Subscription subscription)
    {
        if (context.Subscriptions.Any(sub =>
                sub.ClientId == subscription.ClientId && sub.SoftwareId == subscription.SoftwareId &&
                sub.IsActive))
        {
            throw new ArgumentException("Client already has an active subscription for this software.");
        }

        var discounts = context.Discounts.Where(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now)
            .ToList();
        var applicableDiscount = discounts
            .Where(d => d.OfferType == "subscription").MaxBy(d => d.Value);

        if (applicableDiscount != null)
        {
            subscription.Price -= subscription.Price * applicableDiscount.Value;
        }

        var clientContracts = context.Contracts.Count(c => c.ClientId == subscription.ClientId && c.IsSigned);
        var clientSubscriptions =
            context.Subscriptions.Count(s => s.ClientId == subscription.ClientId && s.IsActive);

        if (clientContracts > 0 || clientSubscriptions > 0)
        {
            subscription.Price -= subscription.Price * 0.05m;
        }

        subscription.NextRenewalDate =
            subscription.StartDate.AddMonths(subscription.RenewalPeriod == "monthly" ? 1 : 12);
        subscription.IsActive = true;

        context.Subscriptions.Add(subscription);
        await context.SaveChangesAsync();

        return subscription;
    }

    public async Task<Subscription?> GetSubscriptionByIdAsync(int id)
    {
        return await context.Subscriptions.FindAsync(id);
    }

    public async Task<bool> UpdateSubscriptionAsync(int id, Subscription subscription)
    {
        if (id != subscription.Id)
        {
            return false;
        }

        context.Entry(subscription).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SubscriptionExists(id))
            {
                return false;
            }

            throw;
        }
    }

    public async Task<bool> DeleteSubscriptionAsync(int id)
    {
        var subscription = await context.Subscriptions.FindAsync(id);
        if (subscription == null)
        {
            return false;
        }

        subscription.IsActive = false;
        context.Entry(subscription).State = EntityState.Modified;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Subscription>> GetAllSubscriptionsAsync()
    {
        return await context.Subscriptions.ToListAsync();
    }

    private bool SubscriptionExists(int id)
    {
        return context.Subscriptions.Any(e => e.Id == id);
    }
}