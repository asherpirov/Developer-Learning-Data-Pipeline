using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataApi.Models;

public class RespondentModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("responseId")]
    public int ResponseId { get; set; }

    [BsonElement("yearsCode")]
    public int? YearsCode { get; set; }

    [BsonElement("learnCode")]
    public List<string>? LearnCode { get; set; }

    [BsonElement("aiLearnHow")]
    public List<string>? AiLearnHow { get; set; }

    [BsonElement("aiAcc")]
    public string? AiAcc { get; set; }

    [BsonElement("aiSelect")]
    public string? AiSelect { get; set; }

    [BsonElement("experienceLevel")]
    public string? ExperienceLevel { get; set; }

    [BsonElement("usesDocumentation")]
    public bool UsesDocumentation { get; set; }

    [BsonElement("usesAIForLearning")]
    public bool UsesAIForLearning { get; set; }

    [BsonElement("usesStackOverflow")]
    public bool UsesStackOverflow { get; set; }
}