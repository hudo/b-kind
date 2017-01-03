using System.Collections.Generic;
using System.Threading.Tasks;
using BKind.Web.Infrastructure;
using BKind.Web.Infrastructure.Persistance;
using BKind.Web.Model;
using BKind.Web.ViewModels;
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

        public async Task<IViewComponentResult> InvokeAsync(StoriesDisplayMode displayMode)
        {
            var stories = await GetStories();

            if (displayMode == StoriesDisplayMode.Featured)
                return View("Default", stories);
            else if (displayMode == StoriesDisplayMode.Latest)
                return View("Latest", stories);
            else
                return null;
        }

        public Task<IEnumerable<Story>> GetStories()
        {
            return Task.FromResult(_db.Stories);
        }
    }
}