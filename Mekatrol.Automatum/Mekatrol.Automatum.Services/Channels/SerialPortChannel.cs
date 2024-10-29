using System.IO.Ports;

namespace Mekatrol.Automatum.Services.Channels;

public class SerialPortChannel(SerialPort serialPort) : ICommunicationChannel
{
    public string Key { get; internal set; } = string.Empty;

    public CommunicationChannelType Type => CommunicationChannelType.Serial;

    public SerialPort SerialPort => serialPort;
}
