using System.Text.Json;
using ProfileApi.Models;

namespace ProfileApi.Services;

public interface IProfileDataService
{
    Task<IndustryExperiencesData?> GetIndustryExperiencesAsync();
    Task<TechnicalSkillsData?> GetTechnicalSkillsAsync();
    Task<WorkExperiencesData?> GetExperiencesAsync();
    Task<CertificationsData?> GetCertificationsAsync();
    Task<EducationsData?> GetEducationsAsync();
    Task<LanguageSkillsData?> GetLanguageSkillsAsync();
    Task<ApplicationSkillsData?> GetApplicationSkillsAsync();
    Task<TechDomainsData?> GetTechDomainsAsync();
    Task<SkillAreasData?> GetSkillAreasAsync();
}

public class ProfileDataService : IProfileDataService
{
    private readonly string _dataPath;
    private readonly ILogger<ProfileDataService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public ProfileDataService(IWebHostEnvironment env, ILogger<ProfileDataService> logger)
    {
        _dataPath = Path.Combine(env.ContentRootPath, "data");
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<IndustryExperiencesData?> GetIndustryExperiencesAsync()
        => await ReadJsonFileAsync<IndustryExperiencesData>("industry-experiences.json");

    public async Task<TechnicalSkillsData?> GetTechnicalSkillsAsync()
        => await ReadJsonFileAsync<TechnicalSkillsData>("tech-stacks.json");

    public async Task<WorkExperiencesData?> GetExperiencesAsync()
        => await ReadJsonFileAsync<WorkExperiencesData>("work-experiences.json");

    public async Task<CertificationsData?> GetCertificationsAsync()
        => await ReadJsonFileAsync<CertificationsData>("certifications.json");

    public async Task<EducationsData?> GetEducationsAsync()
        => await ReadJsonFileAsync<EducationsData>("educations.json");

    public async Task<LanguageSkillsData?> GetLanguageSkillsAsync()
        => await ReadJsonFileAsync<LanguageSkillsData>("language-skills.json");

    public async Task<ApplicationSkillsData?> GetApplicationSkillsAsync()
        => await ReadJsonFileAsync<ApplicationSkillsData>("application-skills.json");

    public async Task<TechDomainsData?> GetTechDomainsAsync()
        => await ReadJsonFileAsync<TechDomainsData>("tech-domains.json");

    public async Task<SkillAreasData?> GetSkillAreasAsync()
        => await ReadJsonFileAsync<SkillAreasData>("skill-areas-and-characteristics.json");

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
