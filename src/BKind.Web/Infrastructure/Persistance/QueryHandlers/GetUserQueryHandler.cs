using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Features.Account.Contracts;
using BKind.Web.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BKind.Web.Infrastructure.Persistance.QueryHandlers
{
    public class GetUserQueryHandler : IAsyncRequestHandler<GetUserQuery, Response<User>>
    {
        private readonly DbContext _db;
        private readonly IMemoryCache _cache;

        public GetUserQueryHandler(DbContext db, IMemoryCache cache)
        {
            _db = db;
            _cache = cache;
        }

        public async Task<Response<User>> Handle(GetUserQuery message)
        {
            var key = CacheKeys.UserWithUsername(message.Username);

            if (_cache.TryGetValue(key, out User user))
                return Response.From(user);

            user = await _db.Set<User>().AsNoTracking()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Username == message.Username);

            if (user != null)
                _cache.Set(key, user);

            return Response.From(user);
        }
    }
}