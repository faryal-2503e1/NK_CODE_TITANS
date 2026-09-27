using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FanHubPlus.Services
{
    public interface ICloudinaryService
    {
        Task<string?> UploadImageAsync(IFormFile file, string folder = "fanhub");
        Task<bool> DeleteImageAsync(string publicId);
    }
}
