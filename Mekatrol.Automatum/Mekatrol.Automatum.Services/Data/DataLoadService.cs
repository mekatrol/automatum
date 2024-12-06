using Mekatrol.Automatum.Middleware.Extensions;
using Mekatrol.Automatum.Models;
using Mekatrol.Automatum.Services.Configuration;
using Mekatrol.Automatum.Utilities.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Mekatrol.Automatum.Services.Data;

internal class DataLoadService(
    ILogger<DataLoadService> logger,
    IOptions<DataStoreConfiguration> options,
    IDataService dataService,
    IDataLockService dataLockService,
    IDataMonitor dataMonitorService) : BackgroundService
{
    private readonly string _dataStorePath = Path.GetFullPath(options.Value.Path);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Does data need loading?
                if (!dataMonitorService.DataLoaded)
                {
                    // Try and load data
                    await LoadData(stoppingToken);
                }

                // Sleep for a second
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch(TaskCanceledException) { /* This exception is normal when shutting down */ }
        catch (Exception ex)
        {
            logger.LogError(ex);
            return;
        }
    }

    private async Task LoadData(CancellationToken stoppingToken)
    {
        using (var dataLock = dataLockService.GetLock(DataLocks.AppData))
        {
            try
            {
                bool result = await LoadDevices(stoppingToken);
                result = await LoadPoints(stoppingToken);
                result = await LoadFlows(stoppingToken);

            }
            catch (Exception ex)
            {
                logger.LogError(ex);
            }
        }

        dataMonitorService.SetDataLoaded();
    }

    private async Task<bool> LoadFlows(CancellationToken stoppingToken)
    {
        var flowsPath = Path.Combine(_dataStorePath, DataService.DirectoryFlows);

        var flowFileNames = Directory.GetFiles(flowsPath, "*.json");

        var success = true;

        dataService.Flows.Clear();

        foreach (var flowFile in flowFileNames)
        {
            try
            {
                var content = await File.ReadAllTextAsync(flowFile, stoppingToken);
                var flow = JsonSerializer.Deserialize<Flow>(content, JsonSerializerExtensions.ApiSerializerOptions)!;
                dataService.Flows.Add(flow.Id, flow);
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                success = false;
            }
        }

        return success;
    }

    private async Task<bool> LoadPoints(CancellationToken stoppingToken)
    {
        var flowsPath = Path.Combine(_dataStorePath, DataService.DirectoryPoints);

        var flowFileNames = Directory.GetFiles(flowsPath, "*.json");

        foreach (var flowFile in flowFileNames)
        {
            try
            {
                var content = await File.ReadAllTextAsync(flowFile, stoppingToken);
                var flow = JsonSerializer.Deserialize<Flow>(content, JsonSerializerExtensions.ApiSerializerOptions);
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
            }
        }

        return true;
    }

    private async Task<bool> LoadDevices(CancellationToken stoppingToken)
    {
        var flowsPath = Path.Combine(_dataStorePath, DataService.DirectoryDevices);

        var flowFileNames = Directory.GetFiles(flowsPath, "*.json");

        foreach (var flowFile in flowFileNames)
        {
            try
            {
                var content = await File.ReadAllTextAsync(flowFile, stoppingToken);
                var flow = JsonSerializer.Deserialize<Flow>(content, JsonSerializerExtensions.ApiSerializerOptions);
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
            }
        }

        return true;
    }
}
