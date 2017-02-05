using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Core.StandardQueries
{
    public class GetOneQuery<T> : IRequest<T> where T:Entity 
    {
        public GetOneQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}