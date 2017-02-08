using System.Threading.Tasks;
using BKind.Web.Model;

namespace BKind.Web.Core
{
    // http://martinfowler.com/eaaCatalog/unitOfWork.html

    public interface IUnitOfWork
    {
        void Add<T>(T entity) where T:Entity;
        void Update<T>(T entity) where T : Entity;
        void Delete<T>(T entity) where T:Entity;

        Task Commit();
    }
}