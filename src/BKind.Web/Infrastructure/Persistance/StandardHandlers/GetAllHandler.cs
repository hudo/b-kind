using System.Linq;
using System.Threading.Tasks;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BKind.Web.Infrastructure.Persistance.StandardHandlers
{
    public class GetAllHandler<T> : IAsyncRequestHandler<GetAllQuery<T>, PagedList<T>> where T : Entity
    {
        private readonly DbContext _db;

        public GetAllHandler(DbContext db)
        {
            _db = db;
        }

        public async Task<PagedList<T>> Handle(GetAllQuery<T> message)
        {
            var query = _db.Set<T>().AsNoTracking();

            query = query.Where(message.Where);

            int count = 0;

            if (message.PageOption != null)
            {
                count = await query.CountAsync();

                query = message.PageOption.Ascending 
                    ? query.OrderBy(message.PageOption.OrderBy) 
                    : query.OrderByDescending(message.PageOption.OrderBy);

                query = query
                    .Skip((message.PageOption.Page - 1) * message.PageOption.PageSize)
                    .Take(message.PageOption.PageSize);
            }

            var entities = await query.ToListAsync();

            return new PagedList<T>(entities, message.PageOption?.Page ?? 0, message.PageOption?.PageSize ?? 0, count);
        }
    }
}