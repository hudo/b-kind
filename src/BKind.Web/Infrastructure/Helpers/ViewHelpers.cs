using System.Threading.Tasks;
using BKind.Web.ViewComponents;
using BKind.Web.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BKind.Web.Infrastructure.Helpers
{
    public static class ViewHelpers
    {
        public static async Task<IHtmlContent> RenderStories(
            this IViewComponentHelper component, 
            StoriesDisplayMode mode)
        {
           
            return await component.InvokeAsync("Stories", new { displayModel = mode });
        }
    }
}