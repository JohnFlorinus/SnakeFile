namespace SnakeFileAPI.Repositories
{
    public interface IFileRepository
    {
        public Task<string> UploadFile(IFormFile file);
        public Task<byte[]> DownloadFile(string ID);
    }
}
