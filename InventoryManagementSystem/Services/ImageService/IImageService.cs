namespace InventoryManagementSystem.Services.ImageService
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile file, string folderPath);
        void DeleteImage(string relativePath, string folderPath);
        Task<string> UpdateImageAsync(IFormFile file, string oldRelativePath, string folderPath);
    }
}
