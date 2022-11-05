namespace AspNetWebApiPractices.Services.Files
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string pathFile);
    }
}