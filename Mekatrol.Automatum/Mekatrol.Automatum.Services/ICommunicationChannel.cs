namespace Mekatrol.Automatum.Services;

public interface ICommunicationChannel
{
    /// <summary>
    /// The unique identifier for this channe, eg 'COM5' or '/dev/ttyACMA0' for serial, '192.168.0.1:80' for TCP/IP
    /// </summary>
    string Key { get; }

    /// <summary>
    /// The channel type so that user of channel can ensure it matches the needed type.
    /// </summary>
    CommunicationChannelType Type { get; }
}