namespace Mekatrol.Automatum.Models;

public class FlowConnection
{
    // Connection configiration properties
    public Guid StartBlockId { get; set; } = Guid.Empty;
    public int StartPin {  get; set; }

    public Guid EndBlockId { get; set; } = Guid.Empty;
    public int EndPin { get; set; }

    // Visual properties for display in user interface
    public bool Selected { get; set; }
}
