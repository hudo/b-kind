using BKind.Web.Core;
using BKind.Web.Features.Shared;
using BKind.Web.Infrastructure;
using MediatR;

namespace BKind.Web.Features.News.Models
{
    public class NewsInputModel : ViewModelBase, IRequest<Response<Model.News>>
    {
        public Model.News News { get; set; }
    }
}