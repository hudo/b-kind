using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IViewComponentResult> InvokeAsync(StoriesDisplayMode mode)
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