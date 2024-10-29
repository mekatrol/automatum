using Mekatrol.Automatum.Models;
using Mekatrol.Automatum.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mekatrol.Automatum.NodeServer.Controllers;

[ApiController]
[Route("flows")]
public class FlowsController(ILogger<DataController> logger, IFlowService flowService) : ControllerBase
{
    [HttpGet]
    public async Task<IList<FlowSummary>> Get(CancellationToken cancellationToken)
    {
        logger.LogDebug("Getting flows...");
        var flows = await flowService.ReadFlowSummaries(cancellationToken);
        return flows;
    }

    [HttpGet("{id}")]
    public async Task<Flow> Get(Guid id, CancellationToken cancellationToken)
    {
        logger.LogDebug("{message}", $"Getting flow with ID '${id}'");
        var flow = await flowService.ReadFlow(id, cancellationToken);
        return flow;
    }

    [HttpPost]
    public async Task<Flow> Post([FromBody] Flow flow, CancellationToken cancellationToken)
    {
        logger.LogDebug("{message}", $"Creating flow with ID '{flow.Id}'");
        flow = await flowService.CreateFlow(flow, cancellationToken);
        return flow;
    }

    [HttpPut]
    public async Task<Flow> Put([FromBody] Flow flow, CancellationToken cancellationToken)
    {
        logger.LogDebug("{message}", $"Saving flow with ID '{flow.Id}'");
        flow = await flowService.UpdateFlow(flow, cancellationToken);
        return flow;
    }

    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
        logger.LogDebug("{message}", $"Deleting flow with ID '${id}'");
        flowService.DeleteFlow(id);
    }
}
