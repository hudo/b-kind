using BKind.Web.Core;
using BKind.Web.ViewModels;
using MediatR;

namespace BKind.Web.Features.News.Models
{
    public class NewsInputModel : ViewModelBase, IRequest<Response<Model.News>>
    {
        public Model.News News { get; set; }
    }
}