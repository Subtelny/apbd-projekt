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

public class ContractsControllerTests
{
    [Fact]
    public async Task GetContracts_ReturnsAllContracts()
    {
        var mockService = new Mock<ContractService>(null!);
        mockService.Setup(service => service.GetAllContractsAsync())
            .ReturnsAsync(GetTestContracts());

        var controller = new ContractsController(mockService.Object);

        var result = await controller.GetContracts();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<Contract>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetContract_ReturnsContract()
    {
        var mockService = new Mock<ContractService>(null!);
        mockService.Setup(service => service.GetContractByIdAsync(1))
            .ReturnsAsync(GetTestContracts().FirstOrDefault(c => c.Id == 1));

        var controller = new ContractsController(mockService.Object);

        var result = await controller.GetContract(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Contract>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
    }

    [Fact]
    public async Task PostContract_CreatesContract()
    {
        var mockService = new Mock<ContractService>(null!);
        var newContract = new Contract
        {
            Id = 3, ClientId = 1, SoftwareId = 1, Price = 1000, StartDate = System.DateTime.Now,
            EndDate = System.DateTime.Now.AddDays(30)
        };
        mockService.Setup(service => service.CreateContractAsync(newContract))
            .ReturnsAsync(newContract);

        var controller = new ContractsController(mockService.Object);

        var result = await controller.PostContract(newContract);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<Contract>(createdAtActionResult.Value);
        Assert.Equal(1000, returnValue.Price);
    }

    [Fact]
    public async Task DeleteContract_DeletesContract()
    {
        var mockService = new Mock<ContractService>(null!);
        var existingContract = GetTestContracts().First();
        mockService.Setup(service => service.DeleteContractAsync(existingContract.Id))
            .ReturnsAsync(true);

        var controller = new ContractsController(mockService.Object);

        var result = await controller.DeleteContract(existingContract.Id);

        Assert.IsType<NoContentResult>(result);
    }

    private List<Contract> GetTestContracts()
    {
        return
        [
            new Contract()
            {
                Id = 1, ClientId = 1, SoftwareId = 1, Price = 1000, StartDate = System.DateTime.Now,
                EndDate = System.DateTime.Now.AddDays(30), IsPaid = true
            },

            new Contract()
            {
                Id = 2, ClientId = 2, SoftwareId = 2, Price = 2000, StartDate = System.DateTime.Now,
                EndDate = System.DateTime.Now.AddDays(60), IsPaid = false
            }
        ];
    }
}