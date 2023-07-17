using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Mock_AWS_API.Interface;

namespace Mock_AWS_API.Services
{
    public class S3BucketService : IS3BucketService
    {
        private readonly IAmazonS3 _s3Client;
        private ILogger<S3BucketService> _logger;

        public S3BucketService(IAmazonS3 s3Client, ILogger<S3BucketService> logger)
        {
            _s3Client = s3Client;
            _logger = logger;
        }
        
      

        public bool uploadFile(IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}
