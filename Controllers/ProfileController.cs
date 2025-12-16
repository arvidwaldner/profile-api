using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ProfileApi.Services;

namespace ProfileApi.Controllers;

[ApiController]
[Route("profile/api")]
[Authorize]
[EnableRateLimiting("fixed")]
public class ProfileController : ControllerBase
{
    private readonly IProfileDataService _dataService;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(IProfileDataService dataService, ILogger<ProfileController> logger)
    {
        _dataService = dataService;
        _logger = logger;
    }
    
    /// <summary>
    /// Get industry experiences
    /// </summary>
    [HttpGet("industry-experiences")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetIndustryExperiences()
    {
        var data = await _dataService.GetIndustryExperiencesAsync();
        return data != null ? Ok(data) : NotFound();
    }

    /// <summary>
    /// Get technical skills/tech stacks
    /// </summary>
    [HttpGet("tech-stacks")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTechStacks()
    {
        var data = await _dataService.GetTechnicalSkillsAsync();
        return data != null ? Ok(data) : NotFound();
    }

    /// <summary>
    /// Get work experiences
    /// </summary>
    [HttpGet("work-experiences")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetExperiences()
    {
        var data = await _dataService.GetExperiencesAsync();
        return data != null ? Ok(data) : NotFound();
    }

    /// <summary>
    /// Get certifications
    /// </summary>
    [HttpGet("certifications")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCertifications()
    {
        var data = await _dataService.GetCertificationsAsync();
        return data != null ? Ok(data) : NotFound();
    }

    /// <summary>
    /// Get educations
    /// </summary>
    [HttpGet("educations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEducations()
    {
        var data = await _dataService.GetEducationsAsync();
        return data != null ? Ok(data) : NotFound();
    }

    /// <summary>
    /// Get language skills
    /// </summary>
    [HttpGet("language-skills")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLanguageSkills()
    {
        var data = await _dataService.GetLanguageSkillsAsync();
        return data != null ? Ok(data) : NotFound();
    }

    /// <summary>
    /// Get application skills
    /// </summary>
    [HttpGet("application-skills")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplicationSkills()
    {
        var data = await _dataService.GetApplicationSkillsAsync();
        return data != null ? Ok(data) : NotFound();
    }

    /// <summary>
    /// Get tech domains
    /// </summary>
    [HttpGet("tech-domains")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTechDomains()
    {
        var data = await _dataService.GetTechDomainsAsync();
        return data != null ? Ok(data) : NotFound();
    }

    /// <summary>
    /// Get skill areas and characteristics
    /// </summary>
    [HttpGet("skill-areas")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSkillAreas()
    {
        var data = await _dataService.GetSkillAreasAsync();
        return data != null ? Ok(data) : NotFound();
    }
}
