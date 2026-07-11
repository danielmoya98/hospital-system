using HospitalSystem.Application.Common.Interfaces;
using Minio;
using Minio.DataModel.Args;

namespace HospitalSystem.Infrastructure.Storage;

public class MinioFileStorageService : IFileStorageService
{
    private readonly IMinioClient _minioClient;

    public MinioFileStorageService(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    public async Task<string> UploadFileAsync(string bucketName, string fileName, Stream fileStream, string contentType, CancellationToken cancellationToken = default)
    {
        // 1. Asegurar que el bucket exista, si no, crearlo
        var bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName), cancellationToken);
        if (!bucketExists)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName), cancellationToken);
        }

        // 2. Subir el archivo
        await _minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType(contentType), cancellationToken);

        return $"{bucketName}/{fileName}";
    }

    public async Task<Stream> DownloadFileAsync(string bucketName, string fileName, CancellationToken cancellationToken = default)
    {
        var memoryStream = new MemoryStream();
        
        await _minioClient.GetObjectAsync(new GetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithCallbackStream(stream => stream.CopyTo(memoryStream)), cancellationToken);

        memoryStream.Position = 0;
        return memoryStream;
    }
}