using System;
using System.Security.Claims;
using BKind.Web.Controllers;
using BKind.Web.Features.Stories;
using BKind.Web.Features.Stories.Contracts;
using BKind.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BKind.Test.Unit
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var validator = new CreateStoryModelValidator();

            var result = validator.Validate(new AddOrUpdateStoryInputModel
            {
                StoryTitle = "title",
                Content = "content"
            });

            Assert.True(result.IsValid);
        }

        [Fact]
        public void test2()
        {
            var controller = new HomeController(Mock.Of<IMediator>(), Mock.Of<ILogger<HomeController>>());

            var identity = new ClaimsIdentity();
            //identity.AuthenticationType

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "name"),
                        new Claim(ClaimTypes.WindowsAccountName, "asd"), 
                    }, "win"))
                }
            };


            var actionResult = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(actionResult);
            var model = Assert.IsType<HomePageViewModel>(viewResult.Model);
            Assert.True(model.CanWriteStory);
        }
    }
}

