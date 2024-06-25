using System.Threading.Tasks;
using APBD_PROJEKT.Service;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace APBD_PROJEKT.Tests.Service;

public class RoleServiceTests
{
    [Fact]
    public async Task CreateRoleAsync_CreatesRole()
    {
        var roleManagerMock = new Mock<RoleManager<IdentityRole>>(
            Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
        roleManagerMock.Setup(r => r.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
        roleManagerMock.Setup(r => r.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);

        var userManagerMock = new Mock<UserManager<IdentityUser>>(
            Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);

        var service = new RoleService(roleManagerMock.Object, userManagerMock.Object);

        var result = await service.CreateRoleAsync("Admin");

        Assert.True(result);
        roleManagerMock.Verify(r => r.CreateAsync(It.Is<IdentityRole>(role => role.Name == "Admin")), Times.Once);
    }

    [Fact]
    public async Task AssignRoleToUserAsync_AssignsRole()
    {
        var roleManagerMock = new Mock<RoleManager<IdentityRole>>(
            Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
        roleManagerMock.Setup(r => r.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

        var user = new IdentityUser { UserName = "testuser" };
        var userManagerMock = new Mock<UserManager<IdentityUser>>(
            Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
        userManagerMock.Setup(u => u.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
        userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        var service = new RoleService(roleManagerMock.Object, userManagerMock.Object);

        var result = await service.AssignRoleToUserAsync("testuser", "Admin");

        Assert.True(result);
        userManagerMock.Verify(u => u.AddToRoleAsync(user, "Admin"), Times.Once);
    }
}