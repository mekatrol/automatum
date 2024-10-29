using Mekatrol.Automatum.Models;

namespace Mekatrol.Automatum.Services;

public interface IDataService
{
    IDictionary<Guid, Flow> Flows { get; }
}
