using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Domain.ModelDTO;

namespace Core.Domain.Extensions
{
    public static class Images
    {
        public static async Task UpdateImage(this string path, string root, Guid id, FileDto fileDto)
        {
            var filePath = Path.Combine(root, id.ToString());

            try
            {
                var folderPath = Path.Combine(path, "Images", filePath);

                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                var absPath = id + "." + fileDto.ImageFormat;

                await File.WriteAllBytesAsync(
                    Path.Combine(folderPath, absPath),
                    Convert.FromBase64String(fileDto.Base64Img));
            }
            catch (Exception e)
            {
                throw new APIException(e.Message, HttpStatusCode.NotAcceptable);
            }
        }

        public static async Task<FileDto> GetBase64Image(ImageStore imageStore, string imageTag, string _path)
        {
            if (imageStore == null) return null;

            var filePath = Path.Combine(imageTag, imageStore.Id.ToString());

            var folderPath = Path.Combine(_path, "Images", filePath);
            var absPath = imageStore.Id + "." + imageStore.Extension;
            var imagePath = Path.Combine(folderPath, absPath);

            return new FileDto
            {
                Base64Img = Convert.ToBase64String(await File.ReadAllBytesAsync(imagePath)),
                ImageFormat = imageStore.Extension
            };
        }
    }
}