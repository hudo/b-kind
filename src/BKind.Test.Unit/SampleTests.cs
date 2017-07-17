using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using BKind.Web.Controllers;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BKind.Test.Unit
{
    public class SampleTests
    {
        [Fact]
        public void ValidatorSimpleTest()
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
    }
}

