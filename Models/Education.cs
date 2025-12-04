namespace ProfileApi.Models;

public class Education
{
    public string Degree { get; set; } = string.Empty;
    public string Institution { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}

public class EducationsData
{
    public List<Education> Educations { get; set; } = new();
}
