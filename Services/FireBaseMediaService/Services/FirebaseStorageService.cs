using FirebaseAdmin;
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

        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            var objectName = $"{folderName}/{Guid.NewGuid()}_{file.FileName}";

            using var stream = file.OpenReadStream();
            await _storageClient.UploadObjectAsync(_bucketName, objectName, null, stream, new UploadObjectOptions
            {
                PredefinedAcl = PredefinedObjectAcl.PublicRead
            });

            return $"https://storage.googleapis.com/{_bucketName}/{objectName}";
        }

        public async Task ViewFileAsync(string fileName, string folderName)
        {
            var objectName = $"{folderName}/{fileName}";
            var obj = await _storageClient.GetObjectAsync(_bucketName, objectName);
            if (obj == null)
            {
                throw new FileNotFoundException($"File {fileName} not found in folder {folderName}.");
            }
            // You can return the object or its metadata as needed
            Console.WriteLine($"File {fileName} found in folder {folderName} with size {obj.Size} bytes.");
        }
    }
}
