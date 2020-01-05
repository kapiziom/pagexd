using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pagexd.Repositories
{
    public class StorageAccRepository : IStorageAccRepository
    {
        private readonly CloudStorageAccount storageAccount;
        private readonly CloudBlobContainer photoContainer;
        private readonly CloudBlobClient cloudBlobClient;
        public StorageAccRepository()
        {
            storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=           ;EndpointSuffix=core.windows.net");
            cloudBlobClient = storageAccount.CreateCloudBlobClient();
            photoContainer = cloudBlobClient.GetContainerReference("photos");

            var containerPermissions = photoContainer.GetPermissionsAsync().Result;
            containerPermissions.PublicAccess = BlobContainerPublicAccessType.Container;
            photoContainer.SetPermissionsAsync(containerPermissions).GetAwaiter();
            photoContainer.CreateIfNotExistsAsync().Wait();
        }

        public string SavePhoto(IFormFile file, string name)
        {
            var blob = photoContainer.GetBlockBlobReference($"{name}.jpg");
            using (var stream = file.OpenReadStream())
            {
                Task.WaitAll(blob.UploadFromStreamAsync(stream));
                return blob.Uri.ToString();
            }
        }
        
    }
}
