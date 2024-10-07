using eCommerceAPI.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly string _imageDirectory;

        public ImageService()
        {
            // Set image directory to a subdirectory of the base directory
            _imageDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            if (!Directory.Exists(_imageDirectory))
            {
                Directory.CreateDirectory(_imageDirectory);
            }
        }

        public string GetFileExtensionFromMimeType(string mimeType)
        {
            return mimeType switch
            {
                "image/png" => ".png",
                "image/jpeg" => ".jpg",
                "image/gif" => ".gif",
                _ => throw new NotSupportedException($"Unsupported MIME type: {mimeType}")
            };
        }

        public async Task<string> GetImageBase64(string filePath)
        {
            string fullPath = Path.Combine(_imageDirectory, filePath);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"File not found: {fullPath}");

            try
            {
                byte[] imageBytes = await File.ReadAllBytesAsync(fullPath);
                return Convert.ToBase64String(imageBytes);
            }
            catch (Exception ex)
            {
                throw new IOException($"Error reading image file: {ex.Message}", ex);
            }
        }

        public (string base64Data, string mimeType) ParseBase64Data(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                throw new ArgumentException("Base64 string is required.");

            // Ensure the string contains a comma separating the data and MIME type
            if (!base64String.Contains(','))
                throw new ArgumentException("Invalid Base64 string format.");

            string[] parts = base64String.Split(',');
            if (parts.Length != 2)
                throw new ArgumentException("Invalid Base64 string format.");

            // Extract MIME type and base64 data
            string mimeType = parts[0].Split(':')[1].Split(';')[0];
            string base64Data = parts[1];

            if (string.IsNullOrEmpty(base64Data))
                throw new ArgumentException("Base64 data is missing.");

            return (base64Data, mimeType);
        }

        public async Task<string> SaveImage(string base64Image)
        {
            if (string.IsNullOrEmpty(base64Image))
                throw new ArgumentException("Base64 image data is required.");

            (string base64Data, string mimeType) = ParseBase64Data(base64Image);

            byte[] imageBytes;
            try
            {
                imageBytes = Convert.FromBase64String(base64Data);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("Invalid Base64 format.", ex);
            }

            string fileName = Guid.NewGuid().ToString() + GetFileExtensionFromMimeType(mimeType);
            string filePath = Path.Combine(_imageDirectory, fileName);

            try
            {
                await File.WriteAllBytesAsync(filePath, imageBytes);
            }
            catch (Exception ex)
            {
                throw new IOException($"Error saving image to server: {ex.Message}", ex);
            }

            return fileName;
        }
    }
}
