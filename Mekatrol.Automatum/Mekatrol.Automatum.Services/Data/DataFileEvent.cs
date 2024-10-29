namespace Mekatrol.Automatum.Services.Data;

internal class DataFileEvent
{
    public void FilesChanged(IList<string> fileNames)
    {
        var args = new DataFileEventArgs
        {
            FileNames = fileNames
        };

        DataFileChanged?.Invoke(this, args);
    }

    public event EventHandler<DataFileEventArgs>? DataFileChanged;
}
