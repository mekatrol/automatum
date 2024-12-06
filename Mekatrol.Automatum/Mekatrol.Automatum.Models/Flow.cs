namespace Mekatrol.Automatum.Models;


public class Flow : FlowSummary
{
    public bool Enabled { get; set; }

    public IList<FlowBlock> Blocks { get; set; } = [];

    public IList<FlowConnection> Connections { get; set; } = [];

    public PersistState PersistState { get; set; }
}
