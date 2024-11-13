using System.Data;
using Oci.Common;
using Oci.Common.Auth;
using Oci.ObjectstorageService;
using Oci.ObjectstorageService.Models;
using Oci.ObjectstorageService.Requests;
using Oci.ObjectstorageService.Responses;
using System.IO;

namespace SnakeFileAPI.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly ObjectStorageClient _objectStorageClient;
        private readonly string _namespaceName;
        private readonly string _bucketName = "bucket-snakefile";

        public FileRepository(ObjectStorageClient objectStorageClient)
        {
            _objectStorageClient = objectStorageClient;
            // namespace behövs för object storage interaction
            _namespaceName = _objectStorageClient.GetNamespace(new GetNamespaceRequest()).Result.Value;
        }

        public async Task<byte[]> DownloadFile(string ID)
        {
            var getObjectRequest = new GetObjectRequest
            {
                NamespaceName = _namespaceName,
                BucketName = _bucketName,
                ObjectName = ID
            };
            GetObjectResponse response = await _objectStorageClient.GetObject(getObjectRequest);
            using (var memoryStream = new MemoryStream())
            {
                response.InputStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            // Kolla om filen är över 100 MB
            // Finns en request size limit i controllern också så detta behövs egentligen inte
            long size = file.Length;
            if (size > (100*1000*1000))
            {
                return "File is too large";
            }
            // Skaffa unikt GUID för filen
            // folk kan ladda upp samma filnamn - GUID separerar
            string ID = Guid.NewGuid().ToString() + file.FileName;
            // Object för bucket uppladdning
            var putObjectRequest = new PutObjectRequest
            {
                NamespaceName = _namespaceName,
                BucketName = _bucketName,
                ObjectName = ID,
                PutObjectBody = file.OpenReadStream()
            };
            var response = await _objectStorageClient.PutObject(putObjectRequest);
            return ID;
        }
    }
}