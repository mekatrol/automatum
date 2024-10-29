namespace Mekatrol.Automatum.Services.Data;

public class DataFileEventArgs : EventArgs
{
    public IList<string> FileNames { get; set; } = [];
}