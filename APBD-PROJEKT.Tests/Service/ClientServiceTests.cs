using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD_PROJEKT.database;
using APBD_PROJEKT.model;
using APBD_PROJEKT.Service;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace APBD_PROJEKT.Tests.Service;

public class ClientServiceTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: "ClientServiceTest")
        .Options;

    [Fact]
    public async Task GetAllClientsAsync_ReturnsAllClients()
    {
        await using (var context = new ApplicationDbContext(_options))
        {
            context.Clients.AddRange(GetTestClients());
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationDbContext(_options))
        {
            var service = new ClientService(context);
            var result = await service.GetAllClientsAsync();
            Assert.Equal(2, result.Count());
        }
    }

    [Fact]
    public async Task GetClientByIdAsync_ReturnsClient()
    {
        await using (var context = new ApplicationDbContext(_options))
        {
            context.Clients.AddRange(GetTestClients());
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationDbContext(_options))
        {
            var service = new ClientService(context);
            var result = await service.GetClientByIdAsync(1);
            Assert.Equal(1, result.Id);
        }
    }

    [Fact]
    public async Task CreateClientAsync_AddsClient()
    {
        using (var context = new ApplicationDbContext(_options))
        {
            var service = new ClientService(context);
            var newClient = new IndividualClient
                { Id = 3, FirstName = "Jane", LastName = "Doe", PESEL = "98765432109" };
            var result = await service.CreateClientAsync(newClient);
            var castedResult = (IndividualClient) result;
            Assert.Equal("Jane", castedResult.FirstName);
            Assert.Equal(3, context.Clients.Count());
        }
    }

    [Fact]
    public async Task UpdateClientAsync_UpdatesClient()
    {
        await using (var context = new ApplicationDbContext(_options))
        {
            context.Clients.AddRange(GetTestClients());
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationDbContext(_options))
        {
            var service = new ClientService(context);
            var existingClient = context.Clients.OfType<IndividualClient>().First();
            existingClient.FirstName = "Updated";
            var result = await service.UpdateClientAsync(existingClient.Id, existingClient);
            Assert.True(result);
            Assert.Equal("Updated", context.Clients.OfType<IndividualClient>().First().FirstName);
        }
    }

    [Fact]
    public async Task DeleteClientAsync_DeletesClient()
    {
        await using (var context = new ApplicationDbContext(_options))
        {
            context.Clients.AddRange(GetTestClients());
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationDbContext(_options))
        {
            var service = new ClientService(context);
            var result = await service.DeleteClientAsync(1);
            Assert.True(result);
            Assert.Equal(1, context.Clients.Count());
        }
    }

    private List<Client> GetTestClients()
    {
        return
        [
            new IndividualClient { Id = 1, FirstName = "John", LastName = "Doe", PESEL = "12345678901" },
            new CompanyClient { Id = 2, CompanyName = "Test Company", KRS = "0000123456" }
        ];
    }
}