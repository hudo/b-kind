using System.Threading.Tasks;
using BKind.Web.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BKind.Web.Features.Account
{
    public class GetUserQuery : IRequest<User>
    {
        public GetUserQuery(string username)
        {
            Username = username;
        }

        public string Username { get; set; }

        public class Handler : IAsyncRequestHandler<GetUserQuery, User>
        {
            private readonly DbContext _db;

            public Handler(DbContext db)
            {
                _db = db;
            }

            public async Task<User> Handle(GetUserQuery message)
            {
                return await _db.Set<User>()
                    .AsNoTracking()
                    .Include(x => x.Roles)
                    .FirstOrDefaultAsync(x => x.Username == message.Username);
            }
        }
    }
}