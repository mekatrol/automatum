using Mekatrol.Automatum.Models;

namespace Mekatrol.Automatum.Services.Data;

internal class DataService : IDataService
{
    public const string DirectoryFlows = "flows";
    public const string DirectoryDevices = "devices";
    public const string DirectoryPoints = "points";
    public const string DirectoryDeleted = "deleted";

    private readonly IDictionary<Guid, Flow> _flows = new Dictionary<Guid, Flow>();

    public IDictionary<Guid, Flow> Flows => _flows;
}
