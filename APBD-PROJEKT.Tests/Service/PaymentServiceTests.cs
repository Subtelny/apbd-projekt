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

public class PaymentServiceTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: "PaymentServiceTest")
        .Options;

    [Fact]
    public async Task CreatePaymentAsync_AddsPayment()
    {
        await using (var context = new ApplicationDbContext(_options))
        {
            context.Contracts.AddRange(GetTestContracts());
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationDbContext(_options))
        {
            var service = new PaymentService(context);
            var newPayment = new Payment { Id = 1, ContractId = 1, Amount = 1000, PaymentDate = DateTime.Now };
            var result = await service.CreatePaymentAsync(newPayment);
            Assert.Equal(1000, result.Amount);
            Assert.Equal(1, result.ContractId);
            Assert.Equal(1, context.Payments.Count());
        }
    }

    [Fact]
    public async Task GetPaymentByIdAsync_ReturnsPayment()
    {
        await using (var context = new ApplicationDbContext(_options))
        {
            context.Payments.AddRange(GetTestPayments());
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationDbContext(_options))
        {
            var service = new PaymentService(context);
            var result = await service.GetPaymentByIdAsync(1);
            Assert.Equal(1, result.Id);
        }
    }

    [Fact]
    public async Task DeletePaymentAsync_DeletesPayment()
    {
        await using (var context = new ApplicationDbContext(_options))
        {
            context.Payments.AddRange(GetTestPayments());
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationDbContext(_options))
        {
            var service = new PaymentService(context);
            var result = await service.DeletePaymentAsync(1);
            Assert.True(result);
            Assert.Equal(1, context.Payments.Count());
        }
    }

    private List<Payment> GetTestPayments()
    {
        return
        [
            new Payment { Id = 1, ContractId = 1, Amount = 1000, PaymentDate = DateTime.Now },
            new Payment { Id = 2, ContractId = 2, Amount = 2000, PaymentDate = DateTime.Now }
        ];
    }

    private List<Contract> GetTestContracts()
    {
        return
        [
            new Contract
            {
                Id = 1, ClientId = 1, SoftwareId = 1, Price = 1000, StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30), IsPaid = true
            },

            new Contract
            {
                Id = 2, ClientId = 2, SoftwareId = 2, Price = 2000, StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(60), IsPaid = false
            }
        ];
    }
}