namespace Mekatrol.Automatum.Services.Configuration;

public class DataStoreConfiguration
{
    public const string SectionName = "DataStore";
    
    /// <summary>
    /// The data file source path.
    /// </summary>
    public string Path { get;set; } = string.Empty;
}
