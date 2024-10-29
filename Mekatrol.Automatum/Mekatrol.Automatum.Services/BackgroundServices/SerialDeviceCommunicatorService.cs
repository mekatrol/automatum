using Mekatrol.Automatum.Services.Channels;
using Mekatrol.Automatum.Services.Configuration;
using Mekatrol.Automatum.Utilities.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO.Ports;

namespace Mekatrol.Automatum.Services.BackgroundServices;

internal class SerialDeviceCommunicatorService(ILogger<SerialDeviceCommunicatorService> logger, IServiceProvider services) : BackgroundService()
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var serialPortsConfig = services.GetRequiredService<IOptions<SerialPortsConfiguration>>().Value;
        var devices = services.GetRequiredService<IOptions<DevicesConfiguration>>().Value;
        var factory = services.GetRequiredService<IDeviceFactory>();

        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var deviceConfig in devices.Values)
            {
                if(!stoppingToken.IsCancellationRequested)
                {
                    break;
                }

                logger.LogDebug("{Message}", $"Processing device {deviceConfig.Provider}|{deviceConfig.Product}|{deviceConfig.Address}");

                var serialPortConfig = serialPortsConfig[deviceConfig.CommunicationChannelKey];

                using var device = factory.GetSerialPortDeviceControllerInstance(deviceConfig);
                using var cancellationTokenSource = new CancellationTokenSource();
                using var serialPort = new SerialPort(serialPortConfig.PortName, serialPortConfig.BaudRate, serialPortConfig.Parity, serialPortConfig.DataBits, serialPortConfig.StopBits);

                serialPort.ReadTimeout = serialPortConfig.ReadTimeout;
                serialPort.WriteTimeout = serialPortConfig.WriteTimeout;

                serialPort.Open();

                var channel = new SerialPortChannel(serialPort);

                try
                {
                    var deviceInfo = await device.GetDeviceInfo(channel, cancellationTokenSource.Token);
                    string infoAsText = Convert.ToString(deviceInfo);
                    logger.LogDebug("{Message}", infoAsText);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex);
                }

                serialPort.Close();
            }

            await Task.Delay(2000, stoppingToken);
        }
    }
}
