using System.Collections.Generic;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Model;

namespace BKind.Web.Infrastructure.Persistance
{
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        private readonly IDatabase _db;

        public InMemoryUnitOfWork(IDatabase db)
        {
            _db = db;
        }

        public void AddOrAttach(Entity entity)
        {
            if(entity.Id > 0) return; 
             
            if(entity is User)
                _db.Users.Add((User)entity);

            if(entity is Story)
                _db.Stories.Add((Story)entity);
        }

        public void Delete(Entity entity)
        {
            // todo
        }

        public Task Commit()
        {
            return Task.CompletedTask;
        }

        public Task Rollback()
        {
            return Task.CompletedTask;
        }
    }
}