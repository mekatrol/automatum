using Mekatrol.Automatum.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mekatrol.Automatum.NodeServer.Controllers;

[ApiController]
[Route("data")]
public class DataController(ILogger<DataController> logger, IDataMonitor dataMonitor) : ControllerBase
{
    [HttpGet(Name = "reload-data")]
    public Task Get()
    {
        logger.LogDebug("{Message}", "Reloading configuration data");
        dataMonitor.QueueDataReload();
        return Task.CompletedTask;
    }
}
