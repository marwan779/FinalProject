namespace InventoryManagementSystem.Services.ImageService
{
    public class ImageService: IImageService
    {
        private readonly IWebHostEnvironment _env;

        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveImageAsync(IFormFile file, string folderPath)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var uploadPath = Path.Combine(_env.WebRootPath, folderPath);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return Path.Combine("/", folderPath, fileName).Replace("\\", "/");
        }

        public void DeleteImage(string relativePath, string folderPath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return;

            var fullPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/'));
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public async Task<string> UpdateImageAsync(IFormFile file, string oldRelativePath, string folderPath)
        {
            DeleteImage(oldRelativePath, folderPath);

            return await SaveImageAsync(file, folderPath);
        }
    }
}
