namespace Mekatrol.Automatum.Services.Data;

internal class DataMonitorService(IDataLockService dataLockService) : IDataMonitor
{
    private bool _dataLoaded = false;

    public bool DataLoaded
    {
        get
        {
            using var lockObject = dataLockService.GetLock(DataLocks.AppData);
            var dataLoaded = _dataLoaded;
            return dataLoaded;
        }
    }

    public void QueueDataReload()
    {
        using var lockObject = dataLockService.GetLock(DataLocks.AppData);

        // Flag data not loaded
        _dataLoaded = false;
    }

    public void SetDataLoaded()
    {
        using var lockObject = dataLockService.GetLock(DataLocks.AppData);

        // Flag data loaded
        _dataLoaded = true;
    }
}
