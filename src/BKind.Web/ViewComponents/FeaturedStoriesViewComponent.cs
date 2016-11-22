using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BKind.Web.Infrastructure;
using BKind.Web.Model;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.ViewComponents
{
    public class FeaturedStoriesViewComponent : ViewComponent
    {
        private readonly IDatabase _db;

        public FeaturedStoriesViewComponent(IDatabase db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await GetStories();
            return View(items);
        }

        public Task<IEnumerable<Story>> GetStories()
        {
            return Task.FromResult(_db.Stories);
        }
    }
}