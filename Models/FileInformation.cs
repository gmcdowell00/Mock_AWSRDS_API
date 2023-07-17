using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mock_AWS_API.Models
{
    public class FileInformation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("fileName")]
        public string fileName { get; set; }

        [BsonElement("filePath")]
        public string filePath { get; set; }
    }
}
