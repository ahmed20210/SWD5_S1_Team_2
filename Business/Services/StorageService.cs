using System.Net.Http.Headers;
using Business.Services;

public class StorageService : IStorageService
{
    private readonly string supabaseUrl = "https://usmilhsblkigypmfbmtr.supabase.co";
    private readonly string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InVzbWlsaHNibGtpZ3lwbWZibXRyIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTc0NDgwMjU1MCwiZXhwIjoyMDYwMzc4NTUwfQ.e9MmApRkD8U590hLdGMSksvCKBMS0HszSld5PmHyu6I";
    private readonly string bucketName = "techxpress";

    public async Task<(bool success, string message, string fileUrl)> UploadPhotoAsync(Stream fileStream, string fileName, string contentType)
    {
        var uniqueName = $"{Guid.NewGuid()}_{fileName}";
        var storagePath = $"public/{uniqueName}"; 

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("apikey", supabaseKey);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", supabaseKey);

            var content = new StreamContent(fileStream);
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            var response = await client.PostAsync(
                $"{supabaseUrl}/storage/v1/object/{bucketName}/{storagePath}", content);

            if (response.IsSuccessStatusCode)
            {
                var fileUrl = $"{supabaseUrl}/storage/v1/object/public/{bucketName}/{storagePath}";
                return (true, "Upload successful", fileUrl);
            }
            
            {
                return (false, response.ReasonPhrase, null);
            }
        }
    }


}