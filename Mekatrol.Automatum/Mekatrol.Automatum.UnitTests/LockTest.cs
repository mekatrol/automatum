using Mekatrol.Automatum.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Mekatrol.Automatum.UnitTests;

[TestClass]
public class LockTest : ServicesUnitTest
{
    [TestMethod]
    public void TestTimeout()
    {
        var dataLockService = _serviceProvider.GetRequiredService<IDataLockService>();

        using var manualResetEvent = new ManualResetEvent(false);
        using var cancellationTokenSource = new CancellationTokenSource();

        const string lockKey = "key";
        const int timeout = 1024;

        // Create a thread and acquire the lock
        var thread1 = new Thread(new ThreadStart(async () =>
        {
            // Acquire lock
            using var lockForKey = dataLockService.GetLock(lockKey, 0);

            // Flag thread started and lock acquired
            manualResetEvent.Set();

            try
            {
                // Wait double lock timeout
                await Task.Delay(timeout << 2, cancellationTokenSource.Token);
            }
            catch (TaskCanceledException) { /* This is expected because we cancelled the token in this test */ }
        }));
        thread1.Start();

        // Wait for thread to start and acquire lock
        manualResetEvent.WaitOne();

        try
        {
            // This should throw exception waiting for lock timeout
            using var lock2 = dataLockService.GetLock(lockKey, timeout);

            Assert.Fail("Should not have acquired the lock");
        }
        catch (Exception ex)
        {
            Assert.AreEqual($"Failed to acquire lock '{lockKey}' within {timeout}ms", ex.Message);
        }
        finally
        {
            cancellationTokenSource.Cancel();
        }
    }

    [TestMethod]
    public void TestNoTimeout()
    {
        var dataLockService = _serviceProvider.GetRequiredService<IDataLockService>();

        using var manualResetEvent = new ManualResetEvent(false);
        using var cancellationTokenSource = new CancellationTokenSource();

        const string lockKey = "key";
        const int timeout = 1024;

        // Create a thread and acquire the lock
        var thread1 = new Thread(new ThreadStart(async () =>
        {
            // Acquire lock
            using var lockForKey = dataLockService.GetLock(lockKey, 0);
            // Flag thread started and lock acquired
            manualResetEvent.Set();

            try
            {
                // Wait half the timeout
                await Task.Delay(timeout >> 2, cancellationTokenSource.Token);
            }
            catch (TaskCanceledException) { /* This can be expected because we cancel the token in this test */ }
        }))
        {
            IsBackground = true
        };
        thread1.Start();

        // Wait for thread to start and acquire lock
        manualResetEvent.WaitOne();

        try
        {
            // This should not throw exception waiting for double lock timeout
            using var lock2 = dataLockService.GetLock(lockKey, timeout << 2);
        }
        finally
        {
            cancellationTokenSource.Cancel();
        }
    }
}