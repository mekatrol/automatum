using Mekatrol.Automatum.Services.Configuration;
using Mekatrol.Automatum.Services.Devices.Waveshare;
using System.Collections.Concurrent;

namespace Mekatrol.Automatum.Services.Devices;

internal class DeviceFactory : IDeviceFactory
{
    private readonly IList<string> _providers = [
        nameof(Waveshare)
        ];

    private readonly ConcurrentDictionary<string, IDeviceController> _deviceInstances = new();

    public IDeviceController GetSerialPortDeviceControllerInstance(DeviceConfiguration deviceConfiguration)
    {
        if (!_providers.Contains(deviceConfiguration.Provider))
        {
            throw new Exception($"Invalid provider name '{deviceConfiguration.Provider}'");
        }

        switch (deviceConfiguration.Product)
        {
            case ModbusRtuRelayDevice.Product:
                {
                    if(string.IsNullOrWhiteSpace(deviceConfiguration.Address))
                    {
                        throw new Exception($"Address not set for '{deviceConfiguration.Provider}', '{deviceConfiguration.Product}'");
                    }

                    if (!byte.TryParse(deviceConfiguration.Address, out byte deviceAddress))
                    {
                        throw new Exception($"Invalid address for '{deviceConfiguration.Provider}', '{deviceConfiguration.Product}' with address '{deviceConfiguration.Address}'");
                    }

                    // Get existing or add new and return.
                    // The is combination of provider, product and device address to make it unique.
                    var key = $"{deviceConfiguration.Provider}.{deviceConfiguration.Product}.{deviceAddress}";
                    _deviceInstances.GetOrAdd(key, (_) =>
                    {
                        return new ModbusRtuRelayDevice(deviceAddress);
                    });
                }
                break;

            default:
                // Unknown product
                throw new Exception($"Invalid product '{deviceConfiguration.Product}'");
        }

        return deviceConfiguration.Product switch
        {
            ModbusRtuRelayDevice.Product => new ModbusRtuRelayDevice(byte.Parse(deviceConfiguration.Address)),

            // Unknown product
            _ => throw new Exception($"Invalid product '{deviceConfiguration.Product}'"),
        };
    }
}
