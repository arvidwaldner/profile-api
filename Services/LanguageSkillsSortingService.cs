namespace ProfileApi.Services;

public interface ILanguageSkillsSortingService
{
    List<T> SortByDomainAndProficiency<T>(List<T> languageSkills) where T : ILanguageSkill;
}

public interface ILanguageSkill
{
    string Domain { get; }
    string Writing { get; }
    string Speaking { get; }
}

public class LanguageSkillsSortingService : ILanguageSkillsSortingService
{
    public List<T> SortByDomainAndProficiency<T>(List<T> languageSkills) where T : ILanguageSkill
    {
        return languageSkills
            .GroupBy(x => x.Domain)
            .SelectMany(g => g.OrderBy(x => GetProficiencyOrder(x.Writing, x.Speaking)))
            .ToList();
    }

    private static int GetProficiencyOrder(string writing, string speaking)
    {
        // Use the higher proficiency level between writing and speaking
        var writingLevel = GetLevelValue(writing);
        var speakingLevel = GetLevelValue(speaking);
        
        return Math.Min(writingLevel, speakingLevel); // Lower value = higher proficiency
    }

    private static int GetLevelValue(string level)
    {
        return level.ToLower() switch
        {
            var l when l.Contains("native") => 1,
            var l when l.Contains("professional") => 2,
            var l when l.Contains("fluent") => 2,
            var l when l.Contains("basic") => 3,
            var l when l.Contains("beginner") => 4,
            _ => 5
        };
    }
}
