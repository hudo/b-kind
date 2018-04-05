using BKind.Web.Core;
using BKind.Web.Features.Shared;
using MediatR;

namespace BKind.Web.Areas.Editor.News.Models
{
    public class NewsInputModel : ViewModelBase, IRequest<Response<Model.News>>
    {
        public Model.News News { get; set; }
    }
}