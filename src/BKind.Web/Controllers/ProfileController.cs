using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Controllers
{
    [Authorize]
    public class ProfileController : BkindControllerBase
    {
        public ProfileController(IMediator mediator) : base(mediator)
        {
        }

       
        public async Task<IActionResult> Index()
        {
            var user = await GetLoggedUserAsync();
            return View();
        }

    }
}