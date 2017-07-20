using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using BKind.Web;
using BKind.Web.Controllers;
using BKind.Web.Controllers.Account;
using BKind.Web.Core;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Account.Contracts;
using BKind.Web.Features.Stories;
using BKind.Web.Features.Stories.Contracts;
using BKind.Web.Features.Stories.Domain;
using BKind.Web.Infrastructure.Persistance;
using BKind.Web.Infrastructure.Persistance.QueryHandlers;
using BKind.Web.Model;
using BKind.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using StructureMap.TypeRules;
using Xunit;

namespace BKind.Test.Unit
{
    public class SampleTests
    {
        [Theory]
        [InlineData("","", false)]
        [InlineData("a", "b", true)]
        public void ValidatorSimpleTest(string title, string content, bool success)
        {
            var validator = new CreateStoryModelValidator();

            var result = validator.Validate(new AddOrUpdateStoryInputModel
            {
                StoryTitle = title,
                Content = content
            });

            Assert.Equal(success, result.IsValid);
        }

        [Fact]
        public void ControllerTestWithMoq()
        {
            var controller = new HomeController(Mock.Of<IMediator>(), Mock.Of<ILogger<HomeController>>());

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

        [Fact]
        public async Task ControllerModelState()
        {
            var mediator = new Mock<IMediator>();
            var controller = new AccountController(mediator.Object);
            controller.ModelState.AddModelError("1", "error");

            var response = await controller.Login(new LoginInputModel());

            var viewResult = Assert.IsType<ViewResult>(response);

            mediator.Verify(x => x.Send(It.IsAny<LoginInputModel>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task HandlerTestWithMoqSetups()
        {
            var mediator = new Mock<IMediator>();
            var uow = new Mock<IUnitOfWork>();

            var story = new Story {Status = Status.Draft};

            mediator
                .Setup(x => x.Send(It.IsAny<GetByIdQuery<Story>>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(story));

            var handler = new ChangeStatusCommandHandler(mediator.Object, uow.Object);

            var command = new ChangeStatusCommand(
                1,
                new User {Roles = new List<Role> {new Reviewer()}},
                Status.Published);

            var result = await handler.Handle(command);

            Assert.False(result.HasErrors);
            Assert.Equal(Status.Published, story.Status);

            uow.Verify(x => x.Update(It.IsAny<Story>()));
            uow.Verify(x => x.Commit());
        }

        [Fact]
        public async Task EfCoreInMemoryDbTest()
        {
            var options = new DbContextOptionsBuilder<StoriesDbContext>()
                .UseInMemoryDatabase("asd")
                .Options;

            var db = new StoriesDbContext(options);

            db.Users.Add(new User
            {
                Username = "asd",
                Roles = new List<Role> { new Administrator() }
            });
            db.SaveChanges();

            var handler = new GetUserQueryHandler(db);

            var response = await handler.Handle(new GetUserQuery("asd"));

            Assert.True(response.HasResult);
            Assert.True(response.Result.Is<Administrator>());
        }

        [Fact]
        public async Task Integration()
        {
            var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"../../../../BKind.Test.Unit"));
            var server = new TestServer(new WebHostBuilder()
                .UseContentRoot(path)
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    var manager = new ApplicationPartManager();
                    manager.ApplicationParts.Add(new AssemblyPart(typeof(Startup).GetAssembly()));
                    manager.ApplicationParts.Add(new AssemblyPart(typeof(SampleTests).GetAssembly()));
                    manager.FeatureProviders.Add(new ControllerFeatureProvider());
                    manager.FeatureProviders.Add(new ViewComponentFeatureProvider());
                    services.AddSingleton(manager);
                }));

            var client = server.CreateClient();

            var response = await client.GetAsync("/home/test");
            response.EnsureSuccessStatusCode();
        }
    }

    public class ResponseHandler : DelegatingHandler
    {
        public ResponseHandler()
        {
            
        }



        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }
    }
}

