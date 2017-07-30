using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BKind.Web.Infrastructure.Persistance
{
    public class DbContextTransactionFilter : IAsyncActionFilter
    {
        private readonly StoriesDbContext _db;

        public DbContextTransactionFilter(StoriesDbContext db)
        {
            _db = db;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _db.BeginTransaction();

            try
            {
                await next();
            }
            catch
            {
                _db.Rollback();
                throw;
            }
        }
    }
}