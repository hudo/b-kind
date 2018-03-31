using MediatR;

namespace BKind.Web.Controllers
{
    public class NewsController : BkindControllerBase
    {
        public NewsController(IMediator mediator) : base(mediator)
        {
        }
    }
}