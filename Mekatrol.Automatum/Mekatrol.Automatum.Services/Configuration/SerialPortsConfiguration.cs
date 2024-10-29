namespace Mekatrol.Automatum.Services.Configuration;

public class SerialPortsConfiguration : Dictionary<string, SerialPortCommunicationChannelConfiguration>
{
    public const string SectionName = "SerialPorts";
}