using Mekatrol.Automatum.Middleware.Exceptions;
using Mekatrol.Automatum.Services.BackgroundServices;
using Mekatrol.Automatum.Services.Data;
using Mekatrol.Automatum.Services.Devices;
using Mekatrol.Automatum.Services.Flows;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

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

    public static IServiceCollection AddApiBehaviours(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            //options.SuppressModelStateInvalidFilter = true;
            options.InvalidModelStateResponseFactory = context =>
            {
                var error = context.ModelState
                    .Select(x => x.Value?.Errors
                        .ToList()
                        .First()
                        .ErrorMessage)
                    .Last(); // Take last error, typically most informative and the actual deserialization error

                // Return error if defined
                throw new BadRequestException(error ?? "A model validation error occured without detail of cause of validation");
            };
        });

        return services;
    }
}
