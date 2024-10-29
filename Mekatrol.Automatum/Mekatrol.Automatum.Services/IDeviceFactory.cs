using Mekatrol.Automatum.Services.Configuration;

namespace Mekatrol.Automatum.Services;

public interface IDeviceFactory
{
    IDeviceController GetSerialPortDeviceControllerInstance(DeviceConfiguration deviceConfiguration);
}
