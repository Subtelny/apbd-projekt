using APBD_PROJEKT.database;
using APBD_PROJEKT.model;
using Microsoft.EntityFrameworkCore;

namespace APBD_PROJEKT.Service;

public class PaymentService(ApplicationDbContext context)
{
    public async Task<Payment> CreatePaymentAsync(Payment payment)
    {
        var contract = await context.Contracts.FindAsync(payment.ContractId);
        if (contract == null)
        {
            throw new ArgumentException("Contract not found.");
        }

        if (payment.PaymentDate > contract.EndDate)
        {
            throw new ArgumentException("Payment date is after contract end date.");
        }

        if (payment.Amount != contract.Price)
        {
            throw new ArgumentException("Payment amount does not match contract price.");
        }

        payment.PaymentDate = DateTime.Now;
        context.Payments.Add(payment);

        contract.IsPaid = true;
        context.Entry(contract).State = EntityState.Modified;

        await context.SaveChangesAsync();

        return payment;
    }

    public async Task<Payment?> GetPaymentByIdAsync(int id)
    {
        return await context.Payments.FindAsync(id);
    }

    public async Task<bool> DeletePaymentAsync(int id)
    {
        var payment = await context.Payments.FindAsync(id);
        if (payment == null)
        {
            return false;
        }

        context.Payments.Remove(payment);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Payment>> GetAllPaymentsAsync()
    {
        return await context.Payments.ToListAsync();
    }
}