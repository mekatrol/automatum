namespace Mekatrol.Automatum.Models;

public class FlowConnection
{
    public Guid StartBlockId { get; set; } = Guid.Empty;
    public int StartPin {  get; set; }

    public Guid EndBlockId { get; set; } = Guid.Empty;
    public int EndPin { get; set; }
}
