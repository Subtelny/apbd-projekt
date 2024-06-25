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

public class SoftwareControllerTests
    {
        [Fact]
        public async Task GetSoftware_ReturnsAllSoftware()
        {
            var mockService = new Mock<SoftwareService>(null);
            mockService.Setup(service => service.GetAllSoftwareAsync())
                .ReturnsAsync(GetTestSoftware());

            var controller = new SoftwareController(mockService.Object);

            var result = await controller.GetSoftware();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Software>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetSoftware_ReturnsSoftware()
        {
            var mockService = new Mock<SoftwareService>(null!);
            mockService.Setup(service => service.GetSoftwareByIdAsync(1))
                .ReturnsAsync(GetTestSoftware().FirstOrDefault(s => s.Id == 1));

            var controller = new SoftwareController(mockService.Object);

            var result = await controller.GetSoftware(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Software>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task PostSoftware_CreatesSoftware()
        {
            var mockService = new Mock<SoftwareService>(null!);
            var newSoftware = new Software { Id = 3, Name = "New Software", Description = "New Description", CurrentVersion = "1.0", Category = "New Category" };
            mockService.Setup(service => service.CreateSoftwareAsync(newSoftware))
                .ReturnsAsync(newSoftware);

            var controller = new SoftwareController(mockService.Object);

            var result = await controller.PostSoftware(newSoftware);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Software>(createdAtActionResult.Value);
            Assert.Equal("New Software", returnValue.Name);
        }

        [Fact]
        public async Task PutSoftware_UpdatesSoftware()
        {
            var mockService = new Mock<SoftwareService>(null!);
            var existingSoftware = GetTestSoftware().First();
            mockService.Setup(service => service.UpdateSoftwareAsync(existingSoftware.Id, existingSoftware))
                .ReturnsAsync(true);

            var controller = new SoftwareController(mockService.Object);

            var result = await controller.PutSoftware(existingSoftware.Id, existingSoftware);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteSoftware_DeletesSoftware()
        {
            var mockService = new Mock<SoftwareService>(null!);
            var existingSoftware = GetTestSoftware().First();
            mockService.Setup(service => service.DeleteSoftwareAsync(existingSoftware.Id))
                .ReturnsAsync(true);

            var controller = new SoftwareController(mockService.Object);

            var result = await controller.DeleteSoftware(existingSoftware.Id);

            Assert.IsType<NoContentResult>(result);
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