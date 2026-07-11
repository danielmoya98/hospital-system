namespace HospitalSystem.Application.Common.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(string bucketName, string fileName, Stream fileStream, string contentType, CancellationToken cancellationToken = default);
    Task<Stream> DownloadFileAsync(string bucketName, string fileName, CancellationToken cancellationToken = default);
}