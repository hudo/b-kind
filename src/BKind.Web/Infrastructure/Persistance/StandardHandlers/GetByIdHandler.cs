using System.Threading.Tasks;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BKind.Web.Infrastructure.Persistance.StandardHandlers
{
    public class GetByIdHandler<T> : IAsyncRequestHandler<GetByIdQuery<T>, T> where T:Entity 
    {
        private readonly DbContext _db;

        public GetByIdHandler(DbContext db)
        {
            _db = db;
        }

        public async Task<T> Handle(GetByIdQuery<T> message)
        {
            var entity = await _db.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == message.Id);
            return entity;
        }
    }
}