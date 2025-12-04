namespace ProfileApi.Models;

public class SkillArea
{
    public string Title { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class SkillAreasData
{
    public List<SkillArea> SkillAreasAndCharacteristics { get; set; } = new();
}
