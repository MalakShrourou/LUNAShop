using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Interfaces.Service
{
    public interface IImageService
    { 
        (string base64Data, string mimeType) ParseBase64Data(string base64String);
        Task<string> GetImageBase64(string filePath);
        string GetFileExtensionFromMimeType(string mimeType);
        Task<string> SaveImage(string base64Image);
    }
}
