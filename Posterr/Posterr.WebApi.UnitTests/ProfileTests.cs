using Microsoft.AspNetCore.Mvc;
using Moq;
using Posterr.CoreObjects.AppInterfaces;
using Posterr.WebAPI.Controllers;
using Posterr.WebAPI.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Posterr.WebApi.UnitTests
{
    public class ProfileTests
    {
        [Fact]
        public async Task WHEN_AnInvalidUsernameIsProvided_THEN_SystemShouldAnswer_BadRequestAndMessage()
        {
            var serviceProviderFake = new Mock<IServiceProvider>();
            var postAppFake = new Mock<IPostApplication>();
            var userAppFake = new Mock<IUserApplication>();
            serviceProviderFake.Setup(x=>x.GetService(typeof(IPostApplication))).Returns(postAppFake.Object);
            serviceProviderFake.Setup(x => x.GetService(typeof(IUserApplication))).Returns(userAppFake.Object);
            Profile profile = new Profile(serviceProviderFake.Object);

            var getResult = await profile.Get("");

            Assert.IsType<ActionResult<UserView>>(getResult);
            Assert.IsType<BadRequestObjectResult>(getResult.Result);
            Assert.Equal("This username is invalid.", (getResult.Result as BadRequestObjectResult).Value);
        }
    }
}
