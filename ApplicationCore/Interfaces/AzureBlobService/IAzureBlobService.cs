using ApplicationCore.Models;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.AzureBlobService
{
    public interface IAzureBlobService
    {
        Task<LogicResult<string>> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType);
    }
}
