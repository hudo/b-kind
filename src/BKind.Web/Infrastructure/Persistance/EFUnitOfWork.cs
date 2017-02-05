using System;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Model;
using Microsoft.EntityFrameworkCore;

namespace BKind.Web.Infrastructure.Persistance
{
    public class EfUnitOfWork : IUnitOfWork
    {
        public EfUnitOfWork(DbContext db)
        {
            
        }

        public void AddOrAttach(Entity entity)
        {
            throw new NotImplementedException();
        }

        public Task Commit()
        {
            throw new NotImplementedException();
        }

        public void Delete(Entity entity)
        {
            throw new NotImplementedException();
        }

        public Task Rollback()
        {
            throw new NotImplementedException();
        }
    }
}