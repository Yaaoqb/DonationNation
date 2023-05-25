using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DonationNation.S3
{
    public interface IS3Service
    {
        Task<string> AddFile(IFormFile file, string type);
        Task<string> RemoveFile(string fileURL);
    }
}
