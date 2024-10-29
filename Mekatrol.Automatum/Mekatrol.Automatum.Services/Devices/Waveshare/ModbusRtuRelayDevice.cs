using Mekatrol.Automatum.Entities;
using Mekatrol.Automatum.Services.Channels;
using Mekatrol.Automatum.Utilities;
using System.IO.Ports;

namespace Mekatrol.Automatum.Services.Devices.Waveshare;

internal class ModbusRtuRelayDevice(byte deviceAddress) : IDeviceController
{
    internal const string Provider = nameof(Waveshare);
    internal const string Product = "Modbus RTU Relay";
    internal const string DriverVersion = "1.0.0";

    public string ProductVersion => throw new NotImplementedException();

    public async Task<dynamic> GetDeviceInfo(ICommunicationChannel communicationChannel, CancellationToken cancellationToken)
    {
        if (communicationChannel.Type != CommunicationChannelType.Serial)
        {
            throw new Exception($"'{nameof(ModbusRtuRelayDevice)}' expecting a communication channel of type '{CommunicationChannelType.Serial}' but got type '{communicationChannel.Type}'");
        }

        var serialPortChannel = (SerialPortChannel)communicationChannel;

        var address = await ReadAddress(serialPortChannel.SerialPort, deviceAddress, cancellationToken);
        var version = await ReadVersion(serialPortChannel.SerialPort, deviceAddress, cancellationToken);

        return new
        {
            Address = address,
            Version = version
        };
    }

    public async Task UpdatePoints(ICommunicationChannel communicationChannel, IList<Point> points, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    public async Task WriteAddress(ICommunicationChannel communicationChannel, byte address, CancellationToken cancellationToken)
    {
        if (communicationChannel.Type != CommunicationChannelType.Serial)
        {
            throw new Exception($"'{nameof(ModbusRtuRelayDevice)}' expecting a communication channel of type '{CommunicationChannelType.Serial}' but got type '{communicationChannel.Type}'");
        }

        var serialPortChannel = (SerialPortChannel)communicationChannel;

        await WriteAddress(serialPortChannel.SerialPort, address, cancellationToken);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    // Refer to: https://www.waveshare.com/wiki/Modbus_RTU_Relay#Set_Device_Address_Command
    /*
     * Modbus frame:
     *  >= 3.5 character break | ADDR  | FUNCTION | DATA      | CRC    | >= 3.5 character break
     *                         | 8 bit | 8 bit    | n * 8 bit | 16 bit | 
     */

    private static Task<string> ReadAddress(SerialPort serialPort, byte deviceAddress, CancellationToken cancellationToken)
    {
        byte[] message = [deviceAddress, (byte)Command.ReadAddressOrVersion, /* Read address */ 0x40, 0x00, 0x00, 0x01, 0x00, 0x00];

        SendAndReceive(serialPort, message, message.Length, 7, cancellationToken);

        var address = message[3] << 8 | message[4];

        return Task.FromResult($"{address}");
    }

    private static Task<string> WriteAddress(SerialPort serialPort, byte deviceAddress, CancellationToken cancellationToken)
    {
        byte[] message = [/* Broadcast address */ 0, (byte)Command.WriteAddress, /* Write address */ 0x40, 0x00, 0x00, deviceAddress, 0x00, 0x00];

        SendAndReceive(serialPort, message, message.Length, message.Length, cancellationToken);

        var address = message[4] << 8 | message[5];

        return Task.FromResult($"{address}");
    }

    private static Task<string> ReadVersion(SerialPort serialPort, byte deviceAddress, CancellationToken cancellationToken)
    {
        byte[] message = [deviceAddress, (byte)Command.ReadAddressOrVersion, /* Read version */ 0x80, 0x00, 0x00, 0x01, 0x00, 0x00];

        SendAndReceive(serialPort, message, message.Length, 7, cancellationToken);

        var version = $"{message[3] << 8 | message[4]:D4}";

        var versionString = $"V{version[0..2].TrimStart('0')}.{version[2..4]}";

        return Task.FromResult(versionString);
    }

    /// <summary>
    /// Helper to send message and try to receive expected number of bytes. Will calcualte set CRC to send on way out and
    /// validate received CRC on way in.
    /// </summary>
    private static bool SendAndReceive(
        SerialPort serialPort,
        byte[] message,
        int sendCount,
        int expectedReceiveCount,
        CancellationToken cancellationToken)
    {
        var crc = Crc.ModbusCrc16(message, 0, sendCount - 2);

        message[^2] = (byte)(crc & 0xFF);
        message[^1] = (byte)(crc >> 8 & 0xFF);

        serialPort.Write(message, 0, message.Length);

        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }

        // 7 bytes expected in return
        var count = Read(serialPort, message, expectedReceiveCount, cancellationToken);

        if (count != expectedReceiveCount)
        {
            throw new Exception($"Incorrect amount of bytes received, expecting {expectedReceiveCount} and got {count}");
        }

        if (!ValidateCrc(message, 0, expectedReceiveCount))
        {
            throw new Exception("Invalid CRC received");
        }

        return true;
    }

    /// <summary>
    /// Helper method to read bytes from serial port. Given a serial port will 'read up to' the 
    /// expected number of bytes, we need to loop until all expected bytes read or a timeout occurs
    /// </summary>
    private static int Read(SerialPort serialPort, byte[] message, int expectedCount, CancellationToken cancellationToken)
    {
        var countSoFar = 0;

        while (countSoFar < expectedCount)
        {
            // Return zero read if cancelled
            if (cancellationToken.IsCancellationRequested) { return 0; }

            // Try and read some bytes
            var count = serialPort.Read(message, countSoFar, expectedCount - countSoFar);
            countSoFar += count;
        }

        return countSoFar;
    }

    private static bool ValidateCrc(byte[] message, int offset, int count)
    {
        var crc = Crc.ModbusCrc16(message, offset, count - 2);

        var crcHigh = (byte)(crc & 0xFF);
        var crcLow = (byte)(crc >> 8 & 0xFF);

        return message[count - 2] == crcHigh && message[count - 1] == crcLow;
    }

    private enum Command : byte
    {
        ReadAddressOrVersion = 0x03,
        WriteAddress = 0x06
    }
}
