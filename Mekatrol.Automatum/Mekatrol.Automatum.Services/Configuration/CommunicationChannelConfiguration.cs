namespace Mekatrol.Automatum.Services.Configuration;

public abstract class CommunicationChannelConfiguration(CommunicationChannelType type) : ICommunicationChannelConfiguration
{
    public CommunicationChannelType Type => type;
}
