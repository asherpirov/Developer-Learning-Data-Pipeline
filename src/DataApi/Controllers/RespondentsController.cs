using DataApi.Models;
using DataApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RespondentsController : ControllerBase
{
    private readonly IMongoService _mongoService;

    public RespondentsController(IMongoService mongoService)
    {
        _mongoService = mongoService;
    }

    [HttpGet("uses-documentation")]
    public async Task<ActionResult<List<RespondentModel>>> GetUsesDocumentation() =>
        Ok(await _mongoService.GetUsesDocumentationAsync());

    [HttpGet("uses-doc-and-ai")]
    public async Task<ActionResult<List<RespondentModel>>> GetUsesDocAndAi() =>
        Ok(await _mongoService.GetUsesDocAndAiAsync());

    [HttpGet("filter-by-ai-acc")]
    public async Task<ActionResult<List<RespondentModel>>> GetByAiAcc([FromQuery] string aiAcc) =>
        Ok(await _mongoService.GetByAiAccAsync(aiAcc));

    [HttpGet("filter-by-experience")]
    public async Task<ActionResult<List<RespondentModel>>> GetByExperienceLevel([FromQuery] string level) =>
        Ok(await _mongoService.GetByExperienceLevelAsync(level));

    [HttpGet("top-ai-developers")]
    public async Task<ActionResult<List<RespondentModel>>> GetTopAiDevelopers() =>
        Ok(await _mongoService.GetTopAiDevelopersAsync());
}