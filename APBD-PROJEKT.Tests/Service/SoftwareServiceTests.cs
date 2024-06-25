using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD_PROJEKT.database;
using APBD_PROJEKT.model;
using APBD_PROJEKT.Service;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace APBD_PROJEKT.Tests.Service;

public class SoftwareServiceTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "SoftwareServiceTest")
            .Options;

        [Fact]
        public async Task GetAllSoftwareAsync_ReturnsAllSoftware()
        {
            await using (var context = new ApplicationDbContext(_options))
            {
                context.Softwares.AddRange(GetTestSoftware());
                await context.SaveChangesAsync();
            }

            await using (var context = new ApplicationDbContext(_options))
            {
                var service = new SoftwareService(context);
                var result = await service.GetAllSoftwareAsync();
                Assert.Equal(2, result.Count());
            }
        }

        [Fact]
        public async Task GetSoftwareByIdAsync_ReturnsSoftware()
        {
            await using (var context = new ApplicationDbContext(_options))
            {
                context.Softwares.AddRange(GetTestSoftware());
                await context.SaveChangesAsync();
            }

            await using (var context = new ApplicationDbContext(_options))
            {
                var service = new SoftwareService(context);
                var result = await service.GetSoftwareByIdAsync(1);
                Assert.Equal(1, result.Id);
            }
        }

        [Fact]
        public async Task CreateSoftwareAsync_AddsSoftware()
        {
            await using (var context = new ApplicationDbContext(_options))
            {
                var service = new SoftwareService(context);
                var newSoftware = new Software { Id = 3, Name = "New Software", Description = "New Description", CurrentVersion = "1.0", Category = "New Category" };
                var result = await service.CreateSoftwareAsync(newSoftware);
                Assert.Equal("New Software", result.Name);
                Assert.Equal(3, context.Softwares.Count());
            }
        }

        [Fact]
        public async Task UpdateSoftwareAsync_UpdatesSoftware()
        {
            await using (var context = new ApplicationDbContext(_options))
            {
                context.Softwares.AddRange(GetTestSoftware());
                await context.SaveChangesAsync();
            }

            await using (var context = new ApplicationDbContext(_options))
            {
                var service = new SoftwareService(context);
                var existingSoftware = context.Softwares.First();
                existingSoftware.Name = "Updated";
                var result = await service.UpdateSoftwareAsync(existingSoftware.Id, existingSoftware);
                Assert.True(result);
                Assert.Equal("Updated", context.Softwares.First().Name);
            }
        }

        [Fact]
        public async Task DeleteSoftwareAsync_DeletesSoftware()
        {
            await using (var context = new ApplicationDbContext(_options))
            {
                context.Softwares.AddRange(GetTestSoftware());
                await context.SaveChangesAsync();
            }

            await using (var context = new ApplicationDbContext(_options))
            {
                var service = new SoftwareService(context);
                var result = await service.DeleteSoftwareAsync(1);
                Assert.True(result);
                Assert.Equal(1, context.Softwares.Count());
            }
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