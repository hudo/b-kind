using System.Threading.Tasks;
using BKind.Web.Model;

namespace BKind.Web.Core
{
    // http://martinfowler.com/eaaCatalog/unitOfWork.html

    public interface IUnitOfWork
    {
        void Add(Entity entity);
        void Delete(Entity entity);

        Task Commit();
        Task Rollback();
    }
}