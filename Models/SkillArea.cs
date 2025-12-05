using ProfileApi.Services;

namespace ProfileApi.Models;

public class SkillArea : ISkillArea
{
    public string Title { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int SalesPitchOrder { get; set; }
}