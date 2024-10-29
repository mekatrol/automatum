using System.Data.Common;

namespace Mekatrol.Automatum.Models;

public class FlowBlock
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Label { get; set; }
    public string FunctionType { get; set; } = string.Empty;
    public Offset Offset { get; set; } = new Offset();
    public Size Size { get; set; } = new Size();
    public int ZOrder { get; set; }
    public IList<InputOutput> Io { get; set; } = [];
}
