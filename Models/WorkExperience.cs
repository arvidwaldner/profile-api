namespace ProfileApi.Models;

public class WorkExperience
{
    public string Company { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Technologies { get; set; } = new();
    public List<string> Languages { get; set; } = new();
    public List<string> Databases { get; set; } = new();
    public List<string> Frontend { get; set; } = new();
    public List<string> DataFormats { get; set; } = new();
    public List<string> Tools { get; set; } = new();
    public List<string> CloudPlatforms { get; set; } = new();
    public List<string> VersionControl { get; set; } = new();
    public List<string> Methodologies { get; set; } = new();
    public List<string> Testing { get; set; } = new();
}

public class WorkExperiencesData
{
    public List<WorkExperience> WorkExperiences { get; set; } = new();
}
