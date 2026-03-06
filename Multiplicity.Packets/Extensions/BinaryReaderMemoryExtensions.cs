using System;
using System.IO;

namespace Multiplicity.Packets
{
    internal static class BinaryReaderMemoryExtensions
    {
        public static ReadOnlyMemory<byte> ReadMemory(this BinaryReader reader, int count)
        {
            if (reader is PacketBinaryReader packetReader)
            {
                return packetReader.ReadMemory(count);
            }

            return reader.ReadBytes(count);
        }

        public static ReadOnlyMemory<byte> ReadRemainingMemory(this BinaryReader reader)
        {
            if (reader is PacketBinaryReader packetReader)
            {
                return packetReader.ReadRemainingMemory();
            }

            int remaining = (int)(reader.BaseStream.Length - reader.BaseStream.Position);
            return reader.ReadBytes(remaining);
        }
    }
}
