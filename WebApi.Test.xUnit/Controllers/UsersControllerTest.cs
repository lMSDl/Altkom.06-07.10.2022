using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers;

namespace WebApi.Test.xUnit.Controllers
{
    public class UsersControllerTest
    {
        [Fact]
        public async Task Get_OkWithAllUsers()
        {
            //Arrange
            var service = new Mock<ICrudService<User>>();
            var expectedList = new Fixture().CreateMany<User>();

            service.Setup(x => x.ReadAsync())
                .ReturnsAsync(expectedList);

            var controller = new UsersController(service.Object);

            //Act
            var result = await controller.Get();

            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var resultList = Assert.IsAssignableFrom<IEnumerable<User>>(actionResult.Value);
            Assert.Equal(expectedList, resultList);
        }

        [Fact]
        public async Task Get_ExistingId_OkWithUser()
        {
            //Arrange
            var service = new Mock<ICrudService<User>>();
            var expectedUser = new Fixture().Create<User>();

            service.Setup(x => x.ReadAsync(expectedUser.Id))
                .ReturnsAsync(expectedUser);

            var controller = new UsersController(service.Object);

            //Act
            var result = await controller.Get(expectedUser.Id);

            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var resultUser = Assert.IsAssignableFrom<User>(actionResult.Value);
            Assert.Equal(expectedUser, resultUser);
        }

        /*[Fact]
        public async Task Get_NonExistingId_NotFound()
        {
            //Arrange
            var service = new Mock<ICrudService<User>>();
            var id = new Fixture().Create<int>();

            service.Setup(x => x.ReadAsync(id))
                .ReturnsAsync((User)null!).Verifiable();

            var controller = new UsersController(service.Object);

            //Act
            var result = await controller.Get(id);

            //Assert
            var actionResult = Assert.IsType<NotFoundResult>(result);
            service.Verify();
        }*/
        [Fact]
        public async Task Get_NonExistingId_NotFound()
        {
            await ReturnsNotFound((controller, id) => controller.Get(id));
        }

            [Fact]
        public async Task Delete_ExistingsId_NoContent()
        {

            //Arrange
            var service = new Mock<ICrudService<User>>();
            var expectedUser = new Fixture().Create<User>();

            service.Setup(x => x.ReadAsync(expectedUser.Id))
                .ReturnsAsync(expectedUser);
            service.Setup(x => x.DeleteAsync(expectedUser.Id))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var controller = new UsersController(service.Object);

            //Act
            var result = await controller.Delete(expectedUser.Id);

            //Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
            service.Verify();
        }

        [Fact]
        public async Task Delete_NonExistingId_NotFound()
        {
            await ReturnsNotFound((controller, id) => controller.Delete(id));
        }

        [Fact]
        public async Task Put_NonExistingId_NotFound()
        {
            User user = null;
            await ReturnsNotFound((controller, id) => controller.Put(id, user));
        }

        private async Task ReturnsNotFound(Func<UsersController, int, Task<IActionResult>> func)
        {
            //Arrange
            var service = new Mock<ICrudService<User>>();
            var id = new Fixture().Create<int>();

            service.Setup(x => x.ReadAsync(id))
                .ReturnsAsync((User)null!).Verifiable();

            var controller = new UsersController(service.Object);

            //Act
            var result = await func(controller, id);

            //Assert
            var actionResult = Assert.IsType<NotFoundResult>(result);
            service.Verify();
        }
    }
}
