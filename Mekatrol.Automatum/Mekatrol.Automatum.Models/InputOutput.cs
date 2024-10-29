namespace Mekatrol.Automatum.Models;

public class InputOutput
{
    public int Pin { get; set; }

    public string? Label { get; set; }

    public string? Description { get; set; }

    public InputOutputSignalType Type { get; set; }

    public InputOutputDirection Direction { get; set; }

    public Offset Offset { get; set; } = new Offset();

    public Size Size { get; set; } = new Size();

    public BlockSide Side { get; set; }
}
