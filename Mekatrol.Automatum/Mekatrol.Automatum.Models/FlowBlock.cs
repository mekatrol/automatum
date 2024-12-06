namespace Mekatrol.Automatum.Models;

public class FlowBlock
{
    //   Block identifier metadata
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string? Label { get; set; }


    // Functional properties for execution of flow
    public string FunctionType { get; set; } = string.Empty;
    public IList<InputOutput> Io { get; set; } = [];

    // Visual properties for display in user interface
    public Offset Offset { get; set; } = new Offset();
    public Size Size { get; set; } = new Size();
    public int ZOrder { get; set; }
    public int ZBoost { get; set; }
    public int Z { get; set; }

    public bool Selected { get; set; }

    public bool? DraggingAsNew { get; set; }

    public bool? DragLocationInvalid { get; set; }

    public bool? DragLocationHasBeenValid { get; set; }
}
