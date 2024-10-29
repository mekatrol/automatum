using System.Collections.Concurrent;

namespace Mekatrol.Automatum.Services.Data;

internal class DataLockService : IDataLockService
{
    // Default lock timeout of 2000 ms (2 seconda)
    public const int LockTimeout = 2000;

    // Thread safe list of semaphores
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphores = new();

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        foreach (var semaphore in _semaphores.Values)
        {
            semaphore.Dispose();
        };
    }

    public IDataLock GetLock(string key, int timeout = LockTimeout)
    {
        // Get an existing lock object or add new one
        var semaphore = _semaphores.GetOrAdd(key, new SemaphoreSlim(1, 1));

        // Lock the semaphore
        return new DataLock(semaphore, key, timeout);
    }

    private class DataLock : IDataLock
    {
        private readonly SemaphoreSlim _sempahore;

        public DataLock(SemaphoreSlim semaphore, string key, int timeout)
        {
            if (!semaphore.Wait(timeout))
            {
                throw new Exception($"Failed to acquire lock '{key}' within {timeout}ms");
            }

            _sempahore = semaphore;
        }

        public void Dispose()
        {
            _sempahore.Release();
            GC.SuppressFinalize(this);
        }
    }
}
