using System.Threading.Tasks;
using APBD_PROJEKT.controllers;
using APBD_PROJEKT.model;
using APBD_PROJEKT.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace APBD_PROJEKT.Tests.Controller;

public class RevenueControllerTests
{
    [Fact]
    public async Task GetRevenue_ReturnsRevenueCalculation()
    {
        var mockService = new Mock<RevenueService>(null!);
        mockService.Setup(service => service.CalculateRevenue())
            .ReturnsAsync(new RevenueCalculation { CurrentRevenue = 1000, PredictedRevenue = 2000 });

        var controller = new RevenueController(mockService.Object);

        var result = await controller.GetRevenue();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<RevenueCalculation>(okResult.Value);
        Assert.Equal(1000, returnValue.CurrentRevenue);
        Assert.Equal(2000, returnValue.PredictedRevenue);
    }
}