using System.IO.Ports;

namespace Mekatrol.Automatum.Services;

public interface ISerialPortCommunicationChannelConfiguration : ICommunicationChannelConfiguration
{
    string PortName { get; }
    int BaudRate { get; }
    Parity Parity { get; }
    int DataBits { get; }
    StopBits StopBits { get; }
}
