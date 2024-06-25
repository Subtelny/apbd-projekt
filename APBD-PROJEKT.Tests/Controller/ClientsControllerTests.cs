using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD_PROJEKT.controllers;
using APBD_PROJEKT.model;
using APBD_PROJEKT.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace APBD_PROJEKT.Tests.Controller;

public class ClientsControllerTests
{
    [Fact]
    public async Task GetClients_ReturnsAllClients()
    {
        var mockService = new Mock<ClientService>(null!);
        mockService.Setup(service => service.GetAllClientsAsync())
            .ReturnsAsync(GetTestClients());

        var controller = new ClientsController(mockService.Object);

        var result = await controller.GetClients();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<Client>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetClient_ReturnsClient()
    {
        var mockService = new Mock<ClientService>(null!);
        mockService.Setup(service => service.GetClientByIdAsync(1))
            .ReturnsAsync(GetTestClients().FirstOrDefault(c => c.Id == 1));

        var controller = new ClientsController(mockService.Object);

        var result = await controller.GetClient(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Client>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
    }

    [Fact]
    public async Task PostClient_CreatesClient()
    {
        var mockService = new Mock<ClientService>(null);
        var newClient = new IndividualClient { Id = 3, FirstName = "Jane", LastName = "Doe", PESEL = "98765432109" };
        mockService.Setup(service => service.CreateClientAsync(newClient))
            .ReturnsAsync(newClient);

        var controller = new ClientsController(mockService.Object);

        var result = await controller.PostClient(newClient);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<IndividualClient>(createdAtActionResult.Value);
        Assert.Equal("Jane", returnValue.FirstName);
    }

    [Fact]
    public async Task PutClient_UpdatesClient()
    {
        var mockService = new Mock<ClientService>(null!);
        var existingClient = GetTestClients().First();
        mockService.Setup(service => service.UpdateClientAsync(existingClient.Id, existingClient))
            .ReturnsAsync(true);

        var controller = new ClientsController(mockService.Object);

        var result = await controller.PutClient(existingClient.Id, existingClient);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteClient_DeletesClient()
    {
        var mockService = new Mock<ClientService>(null!);
        var existingClient = GetTestClients().First();
        mockService.Setup(service => service.DeleteClientAsync(existingClient.Id))
            .ReturnsAsync(true);

        var controller = new ClientsController(mockService.Object);

        var result = await controller.DeleteClient(existingClient.Id);

        Assert.IsType<NoContentResult>(result);
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