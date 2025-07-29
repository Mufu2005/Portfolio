using FirebaseAdmin;
using FirebaseMediaService.Models;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace FirebaseMediaService.Services
{
    public class FirebaseStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName = "portfolio-mediadb.firebasestorage.app";

        public FirebaseStorageService()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile("portfolio-mediadb-firebase-adminsdk-fbsvc-52652f4d39.json")
                });
            }

            var credential = GoogleCredential.FromFile("portfolio-mediadb-firebase-adminsdk-fbsvc-52652f4d39.json");
            _storageClient = StorageClient.Create(credential);
        }

        public async Task<MediaDbModel> UploadFileAsync(IFormFile file, string folderName)
        {
            string fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var objectName = $"{folderName}/{fileName}";

            using var stream = file.OpenReadStream();
            await _storageClient.UploadObjectAsync(_bucketName, objectName, null, stream, new UploadObjectOptions
            {
                PredefinedAcl = PredefinedObjectAcl.PublicRead
            });

            var model = new MediaDbModel
            {
                Title = fileName,
                Url = $"https://storage.googleapis.com/{_bucketName}/{objectName}",
                Folder = folderName
            };

            return model;
        }

        public async Task DeleteFileAsync(string fileName, string folderName)
        {
            var objectName = $"{folderName}/{fileName}";
            await _storageClient.DeleteObjectAsync(_bucketName, objectName);
        }
    }
}
