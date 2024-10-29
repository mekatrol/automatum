namespace Mekatrol.Automatum.Services;

public interface IDataMonitor
{
    void QueueDataReload();
    
    void SetDataLoaded();

    bool DataLoaded { get; }
}
