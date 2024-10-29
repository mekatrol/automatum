namespace Mekatrol.Automatum.Services;

public interface ICommunicationChannelConfiguration
{
    /// <summary>
    /// The channel type so that a derived configuration can be determined.
    /// </summary>
    CommunicationChannelType Type { get; }
}
