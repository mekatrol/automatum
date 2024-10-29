namespace Mekatrol.Automatum.Services.Configuration;

public class DeviceConfiguration
{
    public string Provider { get; set; } = string.Empty;

    public string Product { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public Dictionary<string, string> Configuration { get; set; } = [];

    public CommunicationChannelType CommunicationChannelType { get; set; } = CommunicationChannelType.Unknown;

    public string CommunicationChannelKey { get; set; } = string.Empty;
}
