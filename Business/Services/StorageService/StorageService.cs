using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace Business.Services.StorageService
{
    public class StorageService : IStorageService
    {
        private readonly string _supabaseUrl;
        private readonly string _supabaseKey;
        private readonly string _bucketName;

        public StorageService(IConfiguration configuration)
        {
            _supabaseUrl = configuration["Supabase:Url"] ?? throw new ArgumentNullException(nameof(configuration));
            _supabaseKey = configuration["Supabase:Key"] ?? throw new ArgumentNullException(nameof(configuration));
            _bucketName = configuration["Supabase:BucketName"] ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<(bool success, string message, string fileUrl)> UploadPhotoAsync(
            Stream fileStream, string fileName, string contentType)
        {
            var uniqueName = $"{Guid.NewGuid()}_{fileName}";
            var storagePath = $"public/{uniqueName}"; 

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("apikey", _supabaseKey);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _supabaseKey);

                var content = new StreamContent(fileStream);
                content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                var response = await client.PostAsync(
                    $"{_supabaseUrl}/storage/v1/object/{_bucketName}/{storagePath}", content);

                if (response.IsSuccessStatusCode)
                {
                    var fileUrl = $"{_supabaseUrl}/storage/v1/object/public/{_bucketName}/{storagePath}";
                    return (true, "Upload successful", fileUrl);
                }
                
                return (false, response.ReasonPhrase ?? "Upload fail", null);
            }
        }
    }
}
