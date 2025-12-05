namespace ProfileApi.Services;

public interface ICertificationsSortingService
{
    List<T> SortByIssueDateDescending<T>(List<T> certifications) where T : ICertification;
}

public interface ICertification
{
    string IssueDate { get; }
}

public class CertificationsSortingService : ICertificationsSortingService
{
    public List<T> SortByIssueDateDescending<T>(List<T> certifications) where T : ICertification
    {
        return certifications
            .OrderByDescending(x => ParseDate(x.IssueDate))
            .ToList();
    }

    private static DateTime ParseDate(string dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
        {
            return DateTime.MinValue; // Put invalid dates at the end
        }

        // Try parsing YYYY-MM-DD format
        if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", 
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
