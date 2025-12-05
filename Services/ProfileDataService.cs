using System.Text.Json;
using ProfileApi.Models;

namespace ProfileApi.Services;

public interface IProfileDataService
{
    Task<List<IndustryExperience>?> GetIndustryExperiencesAsync();
    Task<List<TechnicalSkill>?> GetTechnicalSkillsAsync();
    Task<List<WorkExperience>?> GetExperiencesAsync();
    Task<List<Certification>?> GetCertificationsAsync();
    Task<List<Education>?> GetEducationsAsync();
    Task<List<LanguageSkill>?> GetLanguageSkillsAsync();
    Task<List<ApplicationSkill>?> GetApplicationSkillsAsync();
    Task<List<TechDomain>?> GetTechDomainsAsync();
    Task<List<SkillArea>?> GetSkillAreasAsync();
}

public class ProfileDataService : IProfileDataService
{
    private readonly string _dataPath;
    private readonly ILogger<ProfileDataService> _logger;
    private readonly ISkillSortingService _skillSortingService;
    private readonly IWorkExperiencesSortingService _workExperiencesSortingService;
    private readonly ICertificationsSortingService _certificationsSortingService;
    private readonly IEducationsSortingService _educationsSortingService;
    private readonly ILanguageSkillsSortingService _languageSkillsSortingService;
    private readonly ISkillAreasSortingService _skillAreasSortingService;
    private readonly JsonSerializerOptions _jsonOptions;

    public ProfileDataService(IWebHostEnvironment env, ISkillSortingService skillSortingService, IWorkExperiencesSortingService workExperiencesSortingService, ICertificationsSortingService certificationsSortingService, IEducationsSortingService educationsSortingService, ILanguageSkillsSortingService languageSkillsSortingService, ISkillAreasSortingService skillAreasSortingService, ILogger<ProfileDataService> logger)
    {
        _dataPath = Path.Combine(env.ContentRootPath, "data");
        _skillSortingService = skillSortingService;
        _workExperiencesSortingService = workExperiencesSortingService;
        _certificationsSortingService = certificationsSortingService;
        _educationsSortingService = educationsSortingService;
        _languageSkillsSortingService = languageSkillsSortingService;
        _skillAreasSortingService = skillAreasSortingService;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = null
        };
    }

    public async Task<List<IndustryExperience>?> GetIndustryExperiencesAsync()
    {
        var data = await ReadJsonFileAsync<List<IndustryExperience>>("industry-experiences.json");
        return data != null ? _skillSortingService.SortByDomainYearsAndLevel(data) : null;
    }

    public async Task<List<TechnicalSkill>?> GetTechnicalSkillsAsync()
    {
        var data = await ReadJsonFileAsync<List<TechnicalSkill>>("tech-stacks.json");
        return data != null ? _skillSortingService.SortByDomainYearsAndLevel(data) : null;
    }

    public async Task<List<WorkExperience>?> GetExperiencesAsync()
    {
        var data = await ReadJsonFileAsync<List<WorkExperience>>("work-experiences.json");
        return data != null ? _workExperiencesSortingService.SortByFromDateDescending(data) : null;
    }

    public async Task<List<Certification>?> GetCertificationsAsync()
    {
        var data = await ReadJsonFileAsync<List<Certification>>("certifications.json");
        return data != null ? _certificationsSortingService.SortByIssueDateDescending(data) : null;
    }

    public async Task<List<Education>?> GetEducationsAsync()
    {
        var data = await ReadJsonFileAsync<List<Education>>("educations.json");
        return data != null ? _educationsSortingService.SortByYearDescending(data) : null;
    }

    public async Task<List<LanguageSkill>?> GetLanguageSkillsAsync()
    {
        var data = await ReadJsonFileAsync<List<LanguageSkill>>("language-skills.json");
        return data != null ? _languageSkillsSortingService.SortByDomainAndProficiency(data) : null;
    }

    public async Task<List<ApplicationSkill>?> GetApplicationSkillsAsync()
    {
        var data = await ReadJsonFileAsync<List<ApplicationSkill>>("application-skills.json");
        return data != null ? _skillSortingService.SortByDomainYearsAndLevel(data) : null;
    }

    public async Task<List<TechDomain>?> GetTechDomainsAsync()
    {
        var data = await ReadJsonFileAsync<List<TechDomain>>("tech-domains.json");
        return data != null ? _skillSortingService.SortByDomainYearsAndLevel(data) : null;
    }

    public async Task<List<SkillArea>?> GetSkillAreasAsync()
    {
        var data = await ReadJsonFileAsync<List<SkillArea>>("skill-areas-and-characteristics.json");
        return data != null ? _skillAreasSortingService.SortBySalesPitchOrder(data) : null;
    }

    private async Task<T?> ReadJsonFileAsync<T>(string fileName) where T : class
    {
        try
        {
            var filePath = Path.Combine(_dataPath, fileName);
            
            if (!File.Exists(filePath))
            {
                _logger.LogWarning("File not found: {FilePath}", filePath);
                return null;
            }

            var jsonString = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<T>(jsonString, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading file: {FileName}", fileName);
            return null;
        }
    }
}
