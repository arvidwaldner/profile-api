namespace ProfileApi.Models;

public class Certification
{
    public string Name { get; set; } = string.Empty;
    public string IssuingOrganization { get; set; } = string.Empty;
    public string IssueDate { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class CertificationsData
{
    public List<Certification> Certifications { get; set; } = new();
}
