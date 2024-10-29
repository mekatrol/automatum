using Mekatrol.Automatum.Services.BackgroundServices;
using Mekatrol.Automatum.Services.Data;
using Mekatrol.Automatum.Services.Devices;
using Mekatrol.Automatum.Services.Flows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mekatrol.Automatum.Services.Extensions;

public static class AppServicesExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<IDataLockService, DataLockService>();
        services.AddSingleton<IDataMonitor, DataMonitorService>();

        services.AddSingleton<IDataService, DataService>();
        services.AddSingleton<IFlowService, FlowService>();
        services.AddSingleton<IDeviceFactory, DeviceFactory>();

        services.AddHostedService<SerialDeviceCommunicatorService>();
        services.AddHostedService<DataLoadService>();

        return services;
    }

}
