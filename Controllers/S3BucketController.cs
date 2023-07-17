using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using Mock_AWS_API.Controllers;
using Mock_AWS_API.Interface;

namespace Mock_AWS_API.Controllers
{
    [ApiController]
    [Route("api/v1/s3")]
    public class S3BucketController : ControllerBase
    {
        private readonly IAmazonS3 _s3Client;
        private readonly ILogger<S3BucketController> _logger;

        public S3BucketController(IAmazonS3 s3Client, ILogger<S3BucketController> logger)
        {
            _s3Client = s3Client;
            _logger = logger;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllBucketsAsync()
        //{
        //    var data = await _s3Client.ListBucketsAsync();
        //    var buckets = data.Buckets.Select(x => x.BucketName).ToList();
        //    return Ok(buckets);
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateBucket(string bucketName)
        //{
        //    try
        //    {
        //        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
        //        if (bucketExists)
        //        {
        //            _logger.LogInformation($"Bucket {bucketName} already exists.");
        //            return BadRequest($"Bucket {bucketName} already exists.");
        //        }

        //        await _s3Client.PutBucketAsync(bucketName);
        //        _logger.LogInformation($"Bucket {bucketName} already exists.");
        //        return Created("buckets", $"Bucket {bucketName} created.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"An error has occured: {ex.Message}");
        //        return BadRequest("Error creating your bucket. Check log");
        //    }
        //}

        [HttpPost("uploadFile")]
        public async Task<IActionResult> UploadFileAsync(IFormFile file, string bucketName, string? prefix)
        {
            var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
            if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
            var request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.TrimEnd('/')}/{file.FileName}",
                InputStream = file.OpenReadStream()
            };
            request.Metadata.Add("Content-Type", file.ContentType);
            await _s3Client.PutObjectAsync(request);
            return Ok($"File {prefix}/{file.FileName} uploaded to S3 successfully!");
        }

        [HttpGet("getAllFiles")]
        public async Task<IActionResult> GetAllFilesAsync(string bucketName, string? prefix)
        {
            var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
            if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
            var request = new ListObjectsV2Request()
            {
                BucketName = bucketName,
                Prefix = prefix
            };
            var result = await _s3Client.ListObjectsV2Async(request);
            var s3Objects = result.S3Objects.Select(s =>
            {
                var urlRequest = new GetPreSignedUrlRequest()
                {
                    BucketName = bucketName,
                    Key = s.Key,
                    Expires = DateTime.UtcNow.AddMinutes(1)
                };
                return new Models.S3ObjectDto()
                {
                    Name = s.Key.ToString(),
                    PresignedUrl = _s3Client.GetPreSignedURL(urlRequest),
                };
            });
            return Ok(s3Objects);
        }

        [HttpGet("preview")]
        public async Task<IActionResult> GetFileByKeyAsync(string bucketName, string key)
        {
            var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
            if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
            var s3Object = await _s3Client.GetObjectAsync(bucketName, key);
            return File(s3Object.ResponseStream, s3Object.Headers.ContentType);
        }

        [HttpDelete("deleteFile")]
        public async Task<IActionResult> DeleteFileAsync(string bucketName, string key)
        {
            var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
            if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist");
            await _s3Client.DeleteObjectAsync(bucketName, key);
            return NoContent();
        }
    }
}
