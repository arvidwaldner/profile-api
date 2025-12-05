namespace ProfileApi.Services;

public interface ISkillSortingService
{
    List<T> SortByDomainYearsAndLevel<T>(List<T> items) where T : ISkillWithDomainAndLevel;
}

public interface ISkillWithDomainAndLevel
{
    string Domain { get; }
    double Years { get; }
    string Level { get; }
}

public class SkillSortingService : ISkillSortingService
{
    public List<T> SortByDomainYearsAndLevel<T>(List<T> items) where T : ISkillWithDomainAndLevel
    {
        return items
            .GroupBy(x => x.Domain)
            .SelectMany(g => g.OrderByDescending(x => x.Years)
                              .ThenBy(x => GetLevelOrder(x.Level)))
            .ToList();
    }

    private static int GetLevelOrder(string level)
    {
        return level.ToLower() switch
        {
            var l when l.Contains("extensive") => 1,
            var l when l.Contains("solid") => 2,
            var l when l.Contains("basic") => 3,
            _ => 4
        };
    }
}
