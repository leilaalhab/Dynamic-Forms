using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DynamicForms.Models
{
    public class FormulaInputPaths
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public required int FormId { get; set; }
        public InputPath[]? Paths { get; set; }
    }
}