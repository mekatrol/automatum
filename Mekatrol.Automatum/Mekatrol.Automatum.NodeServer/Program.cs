using Mekatrol.Automatum.Middleware.Extensions;
using Mekatrol.Automatum.Services.Configuration;
using Mekatrol.Automatum.Services.Extensions;
using Microsoft.VisualBasic;
using System.Text.Json.Serialization;

namespace Mekatrol.Automatum.NodeServer;

public class Program
{
    private const string AppCorsPolicy = nameof(AppCorsPolicy);

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

        // Bind origins configuration
        var originsConfiguration = new OriginsConfiguration();
        builder.Configuration.Bind(OriginsConfiguration.SectionName, originsConfiguration);

        // Add services to the container.
        builder.Services.AddExceptionMiddleware();

        builder.Services.AddAppServices();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: AppCorsPolicy,
                policy =>
                {
                    policy.WithOrigins([.. originsConfiguration]);
                    policy.AllowAnyMethod();
                    policy.AllowAnyHeader();
                });
        });

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
            );

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SchemaFilter<NonNullablePropertiesRequiredSchemaFilter>();
        });

        var app = builder.Build();

        app.UseCors(AppCorsPolicy);

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
