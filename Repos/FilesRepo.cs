using Mock_AWS_API.Interface;
using Mock_AWS_API.Models;
using MongoDB.Driver;

namespace Mock_AWS_API.Repos
{
    //public class FilesRepo : IFilesRepo
    //{
    //    private readonly string _fileDir;
    //    private readonly ILogger<FilesRepo> _logger;
    //    private readonly IMongoCollection<Models.FileInformation> _collection;
      
    //    public FilesRepo(MongoDbContext db, ILogger<FilesRepo> logger)
    //    {
            
    //        _logger = logger;
    //        _fileDir = db.fileDir;
    //        _collection = db.fileUploadCollection;
    //    }


    //    public List<Models.FileInformation> GetAllFiles()
    //    {
    //        try
    //        {
    //            _logger.LogInformation("Retrieving all file uploads");
    //            return _collection.Find<Models.FileInformation>("").ToList();
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError($"Error occured: {ex.Message}");
    //            return null;

    //        }

    //    }

    //    public bool InsertFile(IFormFile file, Models.FileInformation fileInfo)
    //    {
    //        try
    //        {               
    //            _logger.LogInformation("Insert file");
    //            if (string.IsNullOrEmpty(fileInfo.filePath))
    //                fileInfo.filePath = _fileDir;


    //            _collection.InsertOne(fileInfo);
    //            _logger.LogInformation("File inserted successfully");
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {                
    //            _logger.LogError($"Error occured: {ex.Message}");
    //            return false;
    //        }

    //    }

    //    public bool DeleteFileByName(string fileName)
    //    {
    //        try
    //        {
    //            _collection.DeleteOne(x => x.fileName == fileName);
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            return false;
    //        }
    //    }

    //    private void SaveToLocalDir(IFormFile file)
    //    {
    //        string directoryPath = _fileDir;
    //        string fileName = file.FileName; // Specify the file name

    //        string filePath = Path.Combine(directoryPath, fileName);

    //        // Save content to the file
    //        string fileContent = "Hello, World!";
    //        File.WriteAllText(filePath, fileContent);
    //    }
    //}
}
