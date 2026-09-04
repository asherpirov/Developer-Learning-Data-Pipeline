using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataConsumer.Models;

public class ProcessedMessage
{
    [JsonPropertyName("responseId")]
    public string? ResponseId { get; set; }

    [JsonPropertyName("age")]
    public string? Age { get; set; }

    [JsonPropertyName("yearsCode")]
    public int? YearsCode { get; set; }

    [JsonPropertyName("devType")]
    public string? DevType { get; set; }

    [JsonPropertyName("learnCodeChoose")]
    public List<string>? LearnCodeChoose { get; set; }

    [JsonPropertyName("learningMethods")]
    public List<string>? LearningMethods { get; set; }

    [JsonPropertyName("learnCodeAI")]
    public List<string>? LearnCodeAI { get; set; }

    [JsonPropertyName("aiLearningMethods")]
    public List<string>? AILearningMethods { get; set; }

    [JsonPropertyName("aiUsage")]
    public string? AIUsage { get; set; }

    [JsonPropertyName("aiTrust")]
    public string? AITrust { get; set; }

    [JsonPropertyName("aiSentiment")]
    public string? AISentiment { get; set; }

    [JsonPropertyName("experienceLevel")]
    public string? ExperienceLevel { get; set; }

    [JsonPropertyName("usesDocumentation")]
    public bool UsesDocumentation { get; set; }

    [JsonPropertyName("usesAIForLearning")]
    public bool UsesAIForLearning { get; set; }

    [JsonPropertyName("usesStackOverflow")]
    public bool UsesStackOverflow { get; set; }
}