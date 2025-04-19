namespace Business.Services.StorageService;

public interface IStorageService
{
    Task<(bool success, string message, string fileUrl)> UploadPhotoAsync(Stream fileStream, string fileName, string contentType);
}
