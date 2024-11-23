using Mekatrol.Automatum.Models;

namespace Mekatrol.Automatum.Services;
public interface IFlowService
{
    Task<IList<FlowSummary>> GetSummaries(CancellationToken cancellationToken);

    Task<Flow> Get(Guid id, CancellationToken cancellationToken);

    Task<Flow> Create(Flow flow, CancellationToken cancellationToken);
    
    Task<Flow> Update(Flow flow, CancellationToken cancellationToken);
    
    Task Delete(Guid id);
}
