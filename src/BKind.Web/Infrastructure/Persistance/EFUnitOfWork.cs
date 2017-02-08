using System;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BKind.Web.Infrastructure.Persistance
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly ILogger _logger;
        private readonly DbContext _db;

        public EfUnitOfWork(DbContext db, ILogger<EfUnitOfWork> logger)
        {
            _logger = logger;
            _db = db;
        }

        public void Add<T>(T entity) where T:Entity
        {
            AttachIfNeeded(entity);

            _db.Set<T>().Add(entity);
        }

        public void Update<T>(T entity) where T : Entity
        {
            AttachIfNeeded(entity);

            _db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete<T>(T entity) where T : Entity
        {
            AttachIfNeeded(entity);

            _db.Set<T>().Remove(entity);
        }

        public async Task Commit()
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    await _db.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message, e);
                    transaction.Rollback();

                    throw;
                }
            }
        }

        private void AttachIfNeeded<T>(T entity) where T:Entity
        {
            if (_db.Entry(entity).State == EntityState.Detached)
                _db.Set<T>().Attach(entity);
        }
    }
}