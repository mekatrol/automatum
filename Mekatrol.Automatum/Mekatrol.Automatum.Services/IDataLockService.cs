using Mekatrol.Automatum.Services.Data;

namespace Mekatrol.Automatum.Services;

public interface IDataLockService : IDisposable
{
    IDataLock GetLock(string key, int timeout = DataLockService.LockTimeout);
}

