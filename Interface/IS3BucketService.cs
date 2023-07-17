using Microsoft.AspNetCore.Mvc;

namespace Mock_AWS_API.Interface
{
    public interface IS3BucketService
    {
    
        bool uploadFile(IFormFile file);
    }
}
