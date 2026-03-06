using System;
using System.Text;

namespace Multiplicity.Packets
{
    internal static class PacketSerializationHelper
    {
        public static int GetSerializedStringLength(string value)
        {
            value ??= string.Empty;

            int byteCount = Encoding.UTF8.GetByteCount(value);
            return Get7BitEncodedIntLength(byteCount) + byteCount;
        }

        public static int Get7BitEncodedIntLength(int value)
        {
            uint encodedValue = (uint)value;
            int length = 1;

            while (encodedValue >= 0x80)
            {
                encodedValue >>= 7;
                length++;
            }

            return length;
        }

        public static byte[] NormalizeFixedBuffer(byte[] source, int expectedLength)
        {
            byte[] buffer = new byte[expectedLength];
            if (source != null)
            {
                Buffer.BlockCopy(source, 0, buffer, 0, Math.Min(source.Length, expectedLength));
            }

            return buffer;
        }
    }
}