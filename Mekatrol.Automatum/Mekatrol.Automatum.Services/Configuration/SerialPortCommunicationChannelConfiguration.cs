using System.IO.Ports;

namespace Mekatrol.Automatum.Services.Configuration;

public class SerialPortCommunicationChannelConfiguration()
    : CommunicationChannelConfiguration(CommunicationChannelType.Serial), ISerialPortCommunicationChannelConfiguration
{
    public string PortName { get; set; } = string.Empty;

    public int BaudRate { get; set; } = 9600;

    public Parity Parity { get; set; } = Parity.None;

    public int DataBits { get; set; } = 8;

    public StopBits StopBits { get; set; } = StopBits.One;

    public int ReadTimeout { get; set; } = 200;

    public int WriteTimeout { get; set; } = 200;
}
