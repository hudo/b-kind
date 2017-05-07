using System.Threading.Tasks;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BKind.Web.Infrastructure.Persistance.StandardHandlers
{
    public class GetOneHandler<T> : IAsyncRequestHandler<GetOneQuery<T>, T> where T : Entity
    {
        private readonly DbContext _db;

        public GetOneHandler(DbContext db)
        {
            _db = db;
        }

        public async Task<T> Handle(GetOneQuery<T> message)
        {
            var entity = _db.Set<T>().AsNoTracking();

            if (message.Include != null)
                entity = entity.Include(message.Include);

            return await entity.FirstOrDefaultAsync(message.Condition);
        }
    }
}