using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FanHubPlus.Helpers
{
    public static class ImageStorage
    {
        private static readonly Cloudinary _cloudinary = new Cloudinary(new Account(
            "de2gkzwh5",
            "472767282189483",
            "472767282189483"
        )) { Api = { Secure = true } };

        public static async Task<string?> SaveFileAsync(IFormFile? file, string subFolder = "general")
        {
            if (file == null || file.Length == 0) return null;

            try
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = $"fanhub/{subFolder}",
                    Transformation = new Transformation().Quality("auto").FetchFormat("auto")
                };

                var result = await _cloudinary.UploadAsync(uploadParams);
                if (result.SecureUrl != null)
                {
                    return result.SecureUrl.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cloudinary upload exception: {ex.Message}");
            }

            // Fallback to local storage if Cloudinary upload fails
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", subFolder);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName).ToLowerInvariant();
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/{subFolder}/{uniqueFileName}";
        }
    }
}