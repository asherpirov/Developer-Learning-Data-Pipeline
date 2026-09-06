using DataApi.Models;

namespace DataApi.Services;

public interface IMongoService
{
    Task<List<RespondentModel>> GetUsesDocumentationAsync();
    Task<List<RespondentModel>> GetUsesDocAndAiAsync();
    Task<List<RespondentModel>> GetByAiAccAsync(string aiAcc);
    Task<List<RespondentModel>> GetByExperienceLevelAsync(string level);
    Task<List<RespondentModel>> GetTopAiDevelopersAsync();
}