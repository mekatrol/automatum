using Mekatrol.Automatum.Middleware.Exceptions;
using Mekatrol.Automatum.Middleware.Extensions;
using Mekatrol.Automatum.Models;
using Mekatrol.Automatum.Services.Configuration;
using Mekatrol.Automatum.Services.Data;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Mekatrol.Automatum.Services.Flows;

internal class FlowService : IFlowService
{
    private readonly string _dataStorePath;
    private readonly IDataLockService _dataLockService;
    private readonly IDataService _dataService;

    public FlowService(
        IOptions<DataStoreConfiguration> options,
        IDataService dataService,
        IDataLockService dataLockService)
    {
        if (string.IsNullOrWhiteSpace(options.Value.Path) || !Path.Exists(options.Value.Path))
        {
            throw new ArgumentException($"'{nameof(DataLoadService)}' requires '{nameof(DataStoreConfiguration)}' a non-empty value and for the path to exist.");
        }

        _dataStorePath = Path.GetFullPath(options.Value.Path);
        _dataService = dataService;
        _dataLockService = dataLockService;

        Directory.CreateDirectory(Path.Combine(_dataStorePath, DataService.DirectoryDevices));
        Directory.CreateDirectory(Path.Combine(_dataStorePath, DataService.DirectoryFlows));
        Directory.CreateDirectory(Path.Combine(_dataStorePath, DataService.DirectoryPoints));

        Directory.CreateDirectory(Path.Combine(_dataStorePath, DataService.DirectoryDeleted, DataService.DirectoryDevices));
        Directory.CreateDirectory(Path.Combine(_dataStorePath, DataService.DirectoryDeleted, DataService.DirectoryFlows));
        Directory.CreateDirectory(Path.Combine(_dataStorePath, DataService.DirectoryDeleted, DataService.DirectoryPoints));
    }

    public Task<IList<FlowSummary>> GetSummaries(CancellationToken cancellationToken)
    {
        using var dataLock = _dataLockService.GetLock(DataLocks.AppData);

        var flowSummaries = _dataService.Flows.Values.Select(x =>
        {
            return new FlowSummary
            {
                Id = x.Id,
                Label = x.Label,
                Description = x.Description
            };
        })
        .ToList();

        return Task.FromResult((IList<FlowSummary>)flowSummaries);
    }

    public Task<Flow> Get(Guid id, CancellationToken cancellationToken)
    {
        // If the id is an empty GUID then caller wants a new default flow (do not persist)
        if (id == Guid.Empty)
        {
            return Task.FromResult(new Flow
            {
                Id = Guid.NewGuid(),
                PersistState = PersistState.New
            });
        }

        using var dataLock = _dataLockService.GetLock(DataLocks.AppData);

        if (!_dataService.Flows.TryGetValue(id, out var value))
        {
            throw IdNotFoundException(id);
        }

        return Task.FromResult(value);
    }

    public async Task<Flow> Create(Flow flow, CancellationToken cancellationToken)
    {
        if (flow.Id != Guid.Empty)
        {
            throw new BadRequestException($"A new flow must have the ID set to '{Guid.Empty}'");
        }

        using var dataLock = _dataLockService.GetLock(DataLocks.AppData);

        flow.Id = Guid.NewGuid();
        flow.PersistState = PersistState.Unmodified;

        var dataStorePath = Path.GetFullPath(_dataStorePath);
        var flowPath = Path.Combine(dataStorePath, DataService.DirectoryFlows, $"{flow.Id}.json");
        var json = JsonSerializer.Serialize(flow, JsonSerializerExtensions.ApiSerializerOptions);
        await File.WriteAllTextAsync(flowPath, json, cancellationToken);

        // Add the flow to the loaded flows
        _dataService.Flows.Add(flow.Id, flow);

        return JsonSerializer.Deserialize<Flow>(json, JsonSerializerExtensions.ApiSerializerOptions)!;
    }

    public async Task<Flow> Update(Flow flow, CancellationToken cancellationToken)
    {
        using var dataLock = _dataLockService.GetLock(DataLocks.AppData);

        flow.PersistState = PersistState.Unmodified;

        var dataStorePath = Path.GetFullPath(_dataStorePath);
        var flowPath = Path.Combine(dataStorePath, DataService.DirectoryFlows, $"{flow.Id}.json");
        var json = JsonSerializer.Serialize(flow, JsonSerializerExtensions.ApiSerializerOptions);
        await File.WriteAllTextAsync(flowPath, json, cancellationToken);

        // Update the flow in the loaded flows
        _dataService.Flows[flow.Id] = flow;

        return JsonSerializer.Deserialize<Flow>(json, JsonSerializerExtensions.ApiSerializerOptions)!;
    }

    public Task Delete(Guid id)
    {
        // Lock the data service
        using var dataLock = _dataLockService.GetLock(DataLocks.AppData);

        // Make sure it exists
        if (!_dataService.Flows.ContainsKey(id))
        {
            throw IdNotFoundException(id);
        }

        // Save to disk
        var dataStorePath = Path.GetFullPath(_dataStorePath);
        var flowPath = Path.Combine(dataStorePath, DataService.DirectoryFlows);
        File.Delete(flowPath);

        return Task.CompletedTask;
    }

    private static NotFoundException IdNotFoundException(Guid id) => new($"A flow with the ID '{id}' was not found.");
}
