using ApplicationCore.Interfaces.AzureBlobService;
using ApplicationCore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly string accessKey;
        public AzureBlobService(IConfiguration configuration)
        {
            accessKey = configuration.GetConnectionString("BlobStorage");
        }
        public async Task<LogicResult<string>> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                string strContainerName = "uploads";
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                if (strFileName != null && fileData != null)
                {
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(strFileName);
                    cloudBlockBlob.Properties.ContentType = fileMimeType;
                    await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                    return new LogicResult<string>
                    {
                        IsSuccess = true,
                        Data = cloudBlockBlob.Uri.AbsoluteUri
                    };
                }
                return new LogicResult<string>
                {
                    Errors = new List<string> { "There is no file to upload." }
                };
            }
            catch (Exception ex)
            {
                return new LogicResult<string>
                {
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
