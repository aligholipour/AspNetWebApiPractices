namespace AspNetWebApiPractices.Services.Files
{
    public class FileService : IFileService
    {
        public async Task<string> UploadFileAsync(IFormFile file, string pathFile)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            var fileName = DateTime.Now.Ticks + extension;

            var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), pathFile);

            if (!Directory.Exists(pathBuilt))
                Directory.CreateDirectory(pathBuilt);

            var path = Path.Combine(Directory.GetCurrentDirectory(), pathFile, fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }
    }
}
