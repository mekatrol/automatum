using Mekatrol.Automatum.Services.Configuration;
using Mekatrol.Automatum.Services.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Mekatrol.Automatum.UnitTests;

public class ServicesUnitTest
{
    protected readonly IServiceProvider _serviceProvider;

    public ServicesUnitTest()
    {
        var serviceCollection = new ServiceCollection();

        // Inject configuration for data store
        var dataStoreConfiguration = new DataStoreConfiguration
        {
            Path = "./"
        };
        var options = Options.Create(dataStoreConfiguration)!;
        serviceCollection.AddSingleton(options);

        serviceCollection.AddAppServices();

        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}
