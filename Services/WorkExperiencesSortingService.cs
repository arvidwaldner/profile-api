using ProfileApi.Models;

namespace ProfileApi.Services;

public interface IWorkExperiencesSortingService
{
    List<T> SortByFromDateDescending<T>(List<T> experiences) where T : IWorkExperience;
}

public class WorkExperiencesSortingService : IWorkExperiencesSortingService
{
    public List<T> SortByFromDateDescending<T>(List<T> experiences) where T : IWorkExperience
    {
        return experiences
            .OrderByDescending(x => ParseDateOrPresent(x.From))
            .ToList();
    }

    private static DateTime ParseDateOrPresent(string dateString)
    {
        // Handle "Present" or current dates
        if (string.IsNullOrWhiteSpace(dateString) || 
            dateString.Equals("Present", StringComparison.OrdinalIgnoreCase) ||
            dateString.Equals("Current", StringComparison.OrdinalIgnoreCase))
        {
            return DateTime.MaxValue; // Present should be first
        }

        // Try parsing YYYY-MM format
        if (DateTime.TryParseExact(dateString, "yyyy-MM", 
            System.Globalization.CultureInfo.InvariantCulture, 
            System.Globalization.DateTimeStyles.None, 
            out DateTime result))
        {
            return result;
        }

        // Fallback: try general parsing
        if (DateTime.TryParse(dateString, out result))
        {
            return result;
        }

        // If parsing fails, return minimum date (put at end)
        return DateTime.MinValue;
    }
}
