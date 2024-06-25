using APBD_PROJEKT.database;
using APBD_PROJEKT.model;
using Microsoft.EntityFrameworkCore;

namespace APBD_PROJEKT.Service;

public class ContractService(ApplicationDbContext context)
{
    public async Task<IEnumerable<Contract?>> GetAllContractsAsync()
    {
        return await context.Contracts.ToListAsync();
    }

    public async Task<Contract?> GetContractByIdAsync(int id)
    {
        return await context.Contracts.FindAsync(id);
    }

    public async Task<Contract> CreateContractAsync(Contract contract)
    {
        if (context.Contracts.Any(c =>
                c.ClientId == contract.ClientId && c.SoftwareId == contract.SoftwareId && !c.IsSigned))
        {
            throw new ArgumentException("Client already has an active contract for this software.");
        }

        var discounts = context.Discounts.Where(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now)
            .ToList();
        var applicableDiscount = discounts
            .Where(d => d.OfferType == "upfront").MaxBy(d => d.Value);

        if (applicableDiscount != null)
        {
            contract.Price -= contract.Price * applicableDiscount.Value;
        }

        var clientContracts = context.Contracts.Count(c => c.ClientId == contract.ClientId && c.IsSigned);
        var clientSubscriptions =
            context.Subscriptions.Count(s => s.ClientId == contract.ClientId && s.IsActive);

        if (clientContracts > 0 || clientSubscriptions > 0)
        {
            contract.Price -= contract.Price * 0.05m;
        }

        context.Contracts.Add(contract);
        await context.SaveChangesAsync();

        return contract;
    }

    public async Task<bool> DeleteContractAsync(int id)
    {
        var contract = await context.Contracts.FindAsync(id);
        if (contract == null)
        {
            return false;
        }

        if (contract.IsSigned)
        {
            throw new ArgumentException("Cannot delete a signed contract.");
        }

        context.Contracts.Remove(contract);
        await context.SaveChangesAsync();

        return true;
    }
}