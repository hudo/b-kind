using System.Threading.Tasks;
using BKind.Web.ViewComponents;
using BKind.Web.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BKind.Web.Infrastructure.Helpers
{
    public static class HtmlHelpers
    {
        public static async Task<IHtmlContent> RenderStories(this IViewComponentHelper helper, StoriesDisplayMode displayMode)
        {
            return await helper.InvokeAsync<StoriesViewComponent>(new { mode = displayMode });
        }

        
    }
}