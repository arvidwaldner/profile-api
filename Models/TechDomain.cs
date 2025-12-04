namespace ProfileApi.Models;

public class TechDomain
{
    public string Skill { get; set; } = string.Empty;
    public int Years { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public string DomainIcon { get; set; } = string.Empty;
}

public class TechDomainsData
{
    public List<TechDomain> TechDomains { get; set; } = new();
}
