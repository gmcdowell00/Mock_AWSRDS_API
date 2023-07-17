using Mock_AWS_API.Models;

namespace Mock_AWS_API.Interface
{
    public interface IFilesRepo
    {
        List<FileInformation> GetAllFiles();

        bool InsertFile(IFormFile file, FileInformation fileInfo);  
    }
}
