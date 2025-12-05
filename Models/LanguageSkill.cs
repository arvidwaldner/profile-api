using ProfileApi.Services;

namespace ProfileApi.Models;

public class LanguageSkill : ILanguageSkill
{
    public string Skill { get; set; } = string.Empty;
    public string Writing { get; set; } = string.Empty;
    public string Speaking { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public string DomainIcon { get; set; } = string.Empty;
}