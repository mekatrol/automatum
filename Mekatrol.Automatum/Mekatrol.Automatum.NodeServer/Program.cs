using Mekatrol.Automatum.Middleware.Extensions;
using Mekatrol.Automatum.Services.Configuration;
using Mekatrol.Automatum.Services.Extensions;

namespace Mekatrol.Automatum.NodeServer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<HostOptions>(x =>
        {
            x.ServicesStartConcurrently = true;
            x.ServicesStopConcurrently = true;

            // Don't stop host if background service fails
            x.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
        });

        // Inject configuration for data store
        builder.Services.Configure<DataStoreConfiguration>(
            builder.Configuration.GetSection(DataStoreConfiguration.SectionName));

        // Inject configuration for serial ports
        builder.Services.Configure<SerialPortsConfiguration>(
            builder.Configuration.GetSection(SerialPortsConfiguration.SectionName));

        // Inject configuration for devices
        builder.Services.Configure<DevicesConfiguration>(
            builder.Configuration.GetSection(DevicesConfiguration.SectionName));

        // Add services to the container.
        builder.Services.AddExceptionMiddleware();

        builder.Services.AddAppServices();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseExceptionMiddleware();

        app.UseSwagger();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUI(x =>
            {
                x.EnableTryItOutByDefault();
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
