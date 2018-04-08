using System.IO;
using System.Threading.Tasks;
using BKind.Web.Model;

namespace BKind.Web.Services
{
    public interface IStorageService
    {
        Task UploadAsync(Stream content, string fileName, ContentType type);

        Task DeleteAsync(string fileName, ContentType type);
    }
}