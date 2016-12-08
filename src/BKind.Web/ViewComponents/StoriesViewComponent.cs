using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BKind.Web.Infrastructure;
using BKind.Web.Model;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.ViewComponents
{
    public class StoriesViewComponent : ViewComponent
    {
        private readonly IDatabase _db;

        public StoriesViewComponent(IDatabase db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            var items = GetStories().Result;
            return View(items);
        }

        public Task<IEnumerable<Story>> GetStories()
        {
            return Task.FromResult(_db.Stories);
        }
    }
}