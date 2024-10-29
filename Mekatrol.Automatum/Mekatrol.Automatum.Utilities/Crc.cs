namespace Mekatrol.Automatum.Utilities;

public class Crc
{
    public static ushort ModbusCrc16(byte[] message, int offset = 0, int length = -1)
    {
        // Set values if default
        length = length < 0 ? message.Length : length;
        offset = offset < 0 ? 0 : offset;

        // Init CRC
        ushort crc = 0xFFFF;

        // Process each byte
        for (var i = offset; i < length; i++)
        {
            crc ^= message[i];

            // Process each bit
            for (var b = 0; b < 8; b++)
            {
                if ((crc & 0x0001) != 0)
                {
                    crc >>= 1;
                    crc ^= 0xA001;
                }
                else
                {
                    crc >>= 1;
                }
            }
        }

        return crc;
    }
}
