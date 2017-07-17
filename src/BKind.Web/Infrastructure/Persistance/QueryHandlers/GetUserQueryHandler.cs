using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Features.Account.Contracts;
using BKind.Web.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BKind.Web.Infrastructure.Persistance.QueryHandlers
{
    public class GetUserQueryHandler : IAsyncRequestHandler<GetUserQuery, Response<User>>
    {
        private readonly DbContext _db;

        public GetUserQueryHandler(DbContext db)
        {
            _db = db;
        }

        public async Task<Response<User>> Handle(GetUserQuery message)
        {
            return Response.From(await _db.Set<User>().AsNoTracking()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Username == message.Username));
        }
    }
}