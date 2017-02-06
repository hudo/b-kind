using System.Linq;
using System.Threading.Tasks;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BKind.Web.Infrastructure.Persistance.StandardHandlers
{
    public class GetAllHandler<T> : IAsyncRequestHandler<GetAllQuery<T>, T[]> where T : Entity
    {
        private readonly DbContext _db;

        public GetAllHandler(DbContext db)
        {
            _db = db;
        }

        public Task<T[]> Handle(GetAllQuery<T> message)
        {
            var entities = _db.Set<T>().AsNoTracking();

            entities = entities.Where(message.Where);

            if (message.PageOption != null)
            {
                if (message.PageOption.Ascending)
                    entities = entities.OrderBy(message.PageOption.OrderBy);
                else
                    entities = entities.OrderByDescending(message.PageOption.OrderBy);

                entities = entities
                    .Skip((message.PageOption.Page + 1) * message.PageOption.PageSize)
                    .Take(message.PageOption.PageSize);
            }

            return entities.ToArrayAsync();
        }
    }
}