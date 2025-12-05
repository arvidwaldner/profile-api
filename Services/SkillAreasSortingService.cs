namespace ProfileApi.Services;

public interface ISkillAreasSortingService
{
    List<T> SortBySalesPitchOrder<T>(List<T> skillAreas) where T : ISkillArea;
}

public interface ISkillArea
{
    string Title { get; }
    int SalesPitchOrder { get; }
}

public class SkillAreasSortingService : ISkillAreasSortingService
{
    public List<T> SortBySalesPitchOrder<T>(List<T> skillAreas) where T : ISkillArea
    {
        return skillAreas
            .OrderBy(x => x.SalesPitchOrder)
            .ToList();
    }
}
