using DataApi.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace DataApi.Services;

public class MongoService : IMongoService
{
    private readonly IMongoCollection<RespondentModel> _respondents;

    public MongoService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDb") ?? "mongodb://localhost:27017";
        var mongoUrl = MongoUrl.Create(connectionString);
        var mongoClient = new MongoClient(mongoUrl);
        var database = mongoClient.GetDatabase("developer_db");
        _respondents = database.GetCollection<RespondentModel>("respondents");
    }

    public async Task<List<RespondentModel>> GetUsesDocumentationAsync() =>
        await _respondents.Find(r => r.UsesDocumentation).ToListAsync();

    public async Task<List<RespondentModel>> GetUsesDocAndAiAsync() =>
        await _respondents.Find(r => r.UsesDocumentation && r.UsesAIForLearning).ToListAsync();

    public async Task<List<RespondentModel>> GetByAiAccAsync(string aiAcc) =>
        await _respondents.Find(r => r.AiAcc == aiAcc).ToListAsync();

    public async Task<List<RespondentModel>> GetByExperienceLevelAsync(string level) =>
        await _respondents.Find(r => r.ExperienceLevel == level).ToListAsync();

    public async Task<List<RespondentModel>> GetTopAiDevelopersAsync() =>
        await _respondents
            .Find(r => r.UsesAIForLearning)
            .SortByDescending(r => r.YearsCode)
            .Limit(20)
            .ToListAsync();
}