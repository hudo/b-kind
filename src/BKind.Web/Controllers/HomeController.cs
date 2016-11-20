using System;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            throw new Exception("ex");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}