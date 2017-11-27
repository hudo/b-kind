using System.Threading.Tasks;
using BKind.Web.Model;

namespace BKind.Web.Core
{
    // http://martinfowler.com/eaaCatalog/unitOfWork.html

    public interface IUnitOfWork
    {
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;

        Task Commit();
    }
}