namespace ProfileApi.Models;

public class TechnicalSkill
{
    public string Skill { get; set; } = string.Empty;
    public double Years { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public string DomainIcon { get; set; } = string.Empty;
}

public class TechnicalSkillsData
{
    public List<TechnicalSkill> TechnicalSkills { get; set; } = new();
}
