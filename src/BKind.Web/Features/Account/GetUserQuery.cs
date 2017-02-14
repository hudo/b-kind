using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BKind.Web.Features.Account
{
    public class GetUserQuery : IRequest<Response<User>>
    {
        public GetUserQuery(string username)
        {
            Username = username;
        }

        public string Username { get; set; }

        public class Handler : IAsyncRequestHandler<GetUserQuery, Response<User>>
        {
            private readonly DbContext _db;

            public Handler(DbContext db)
            {
                _db = db;
            }

            public async Task<Response<User>> Handle(GetUserQuery message)
            {
                return Response.From(await _db.Set<User>()
                    .AsNoTracking()
                    .Include(x => x.Roles)
                    .FirstOrDefaultAsync(x => x.Username == message.Username));
            }
        }
    }
}