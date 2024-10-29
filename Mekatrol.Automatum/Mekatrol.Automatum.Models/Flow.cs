namespace Mekatrol.Automatum.Models;


public class Flow : FlowSummary
{
    public IList<FlowBlock> Blocks { get; set; } = [];

    public IList<FlowConnection> Connections { get; set; } = [];
}
