using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Controllers
{
    public class StoriesController : BkindControllerBase
    {
        IMediator _mediator;

        public StoriesController(IMediator mediator)
        {
            _mediator = mediator;    
        }

        public IActionResult Share()
        {
            return View();
        }
    }
}
