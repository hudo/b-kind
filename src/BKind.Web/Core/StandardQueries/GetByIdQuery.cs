using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Core.StandardQueries
{
    public class GetByIdQuery<T> : IRequest<T> where T:Entity 
    {
        public GetByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}