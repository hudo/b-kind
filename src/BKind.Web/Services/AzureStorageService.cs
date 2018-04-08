using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Model;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BKind.Web.Services
{
    public class AzureStorageService : IStorageService
    {
        private readonly AppSettings _appSettings;
        private Dictionary<ContentType, string> _contentTypeMapping;

        public AzureStorageService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

            _contentTypeMapping = new Dictionary<ContentType, string>
            {
                { ContentType.Story,_appSettings.StoryPhotoContainer }
            };
        }

        public async Task UploadAsync(Stream content, string fileName, ContentType type)
        {
            var container = GetStorageContainer(_contentTypeMapping[type]);

            var blockBlob = container.GetBlockBlobReference(fileName);

            await blockBlob.UploadFromStreamAsync(content);
        }

        public async Task DeleteAsync(string fileName, ContentType type)
        {
            var container = GetStorageContainer(_contentTypeMapping[type]);

            var blockBlob = container.GetBlockBlobReference(fileName);

            await blockBlob.DeleteAsync();
        }

        private CloudBlobContainer GetStorageContainer(string container)
        {
            var account = CloudStorageAccount.Parse(_appSettings.AzureStorageConnectionString);
            var client = account.CreateCloudBlobClient();
            var reference = client.GetContainerReference(container);
            return reference;
        }


    }
}