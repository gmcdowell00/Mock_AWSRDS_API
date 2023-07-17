using Mock_AWS_API.Interface;
using Mock_AWS_API.Models;
using MongoDB.Driver;

namespace Mock_AWS_API.Repos
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly string _fileDir;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("MongoDB:ConnectionString");     
            var databaseName = configuration.GetValue<string>("MongoDB:DatabaseName");
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
            _fileDir = configuration.GetValue<string>("fileDir");
        }

        public string fileDir => _fileDir;

        public IMongoCollection<Models.FileInformation> fileUploadCollection => _database.GetCollection<Models.FileInformation>("files");
    }
}
