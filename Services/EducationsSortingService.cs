namespace ProfileApi.Services;

public interface IEducationsSortingService
{
    List<T> SortByYearDescending<T>(List<T> educations) where T : IEducation;
}

public interface IEducation
{
    string Year { get; }
}

public class EducationsSortingService : IEducationsSortingService
{
    public List<T> SortByYearDescending<T>(List<T> educations) where T : IEducation
    {
        return educations
            .OrderByDescending(x => ParseYearRange(x.Year))
            .ToList();
    }

    private static int ParseYearRange(string yearString)
    {
        if (string.IsNullOrWhiteSpace(yearString))
        {
            return 0; // Put invalid years at the end
        }

        // Handle year ranges like "2012 - 2014" or "2012-2014"
        var parts = yearString.Split(new[] { '-', '–' }, StringSplitOptions.RemoveEmptyEntries);
        
        if (parts.Length > 0)
        {
            // Get the last year (end year) for sorting
            var lastPart = parts[parts.Length - 1].Trim();
            
            if (int.TryParse(lastPart, out int year))
            {
                return year;
            }
        }

        // Try parsing as single year
        if (int.TryParse(yearString.Trim(), out int singleYear))
        {
            return singleYear;
        }

        // If parsing fails, return 0 (put at end)
        return 0;
    }
}
