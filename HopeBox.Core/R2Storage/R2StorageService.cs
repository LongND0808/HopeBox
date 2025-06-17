using HopeBox.Core.R2Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using System;
using System.IO;
using System.Threading.Tasks;

public class R2StorageService : IR2StorageService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;
    private readonly string _publicUrl;

    public R2StorageService(IConfiguration configuration)
    {
        var accessKey = configuration["CloudflareR2:AccessKey"];
        var secretKey = configuration["CloudflareR2:SecretKey"];
        var accountId = configuration["CloudflareR2:AccountId"];
        _bucketName = configuration["CloudflareR2:Bucket"] ?? "";
        _publicUrl = configuration["CloudflareR2:PublicUrl"] ?? throw new ArgumentNullException("CloudflareR2:PublicUrl");

        _minioClient = new MinioClient()
            .WithEndpoint($"{accountId}.r2.cloudflarestorage.com")
            .WithCredentials(accessKey, secretKey)
            .WithSSL(true)
            .Build();
    }

    public async Task<string> UploadFileAsync(IFormFile file, string keyPrefix, string fileName)
    {
        try
        {
            var fileExt = Path.GetExtension(file.FileName);
            var ServerFileName = $"{keyPrefix}/{fileName}{fileExt}";

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(ServerFileName)
                .WithStreamData(memoryStream)
                .WithObjectSize(memoryStream.Length)
                .WithContentType(file.ContentType);

            await _minioClient.PutObjectAsync(putObjectArgs);

            return $"{_publicUrl.TrimEnd('/')}/{ServerFileName}";
        }
        catch (Exception ex)
        {
            throw new Exception($"Upload failed: {ex.Message}", ex);
        }
    }
}
