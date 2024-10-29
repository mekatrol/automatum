using Mekatrol.Automatum.Models;

namespace Mekatrol.Automatum.Services;
public interface IFlowService
{
    Task<IList<FlowSummary>> ReadFlowSummaries(CancellationToken cancellationToken);

    Task<Flow> ReadFlow(Guid id, CancellationToken cancellationToken);

    Task<Flow> CreateFlow(Flow flow, CancellationToken cancellationToken);
    
    Task<Flow> UpdateFlow(Flow flow, CancellationToken cancellationToken);
    
    Task DeleteFlow(Guid id);
}
