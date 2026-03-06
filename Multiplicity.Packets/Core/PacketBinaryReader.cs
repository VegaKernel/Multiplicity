using System;
using System.IO;
using System.Text;

namespace Multiplicity.Packets
{
    public sealed class PacketBinaryReader : BinaryReader
    {
        private readonly IPacketMemorySource _memorySource;

        public PacketBinaryReader(ReadOnlyMemory<byte> buffer)
            : this(new ReadOnlyMemoryStream(buffer))
        {
        }

        public PacketBinaryReader(PacketTypes packetType, ReadOnlyMemory<byte> payload)
            : this(new PacketPayloadStream((byte)packetType, payload))
        {
        }

        public PacketBinaryReader(byte packetId, ReadOnlyMemory<byte> payload)
            : this(new PacketPayloadStream(packetId, payload))
        {
        }

        private PacketBinaryReader(Stream stream)
            : base(stream, Encoding.UTF8, leaveOpen: true)
        {
            _memorySource = (IPacketMemorySource)stream;
        }

        public ReadOnlyMemory<byte> ReadMemory(int count)
        {
            return _memorySource.ReadMemory(count);
        }

        public ReadOnlyMemory<byte> ReadRemainingMemory()
        {
            int remaining = (int)(_memorySource.Length - _memorySource.Position);
            return _memorySource.ReadMemory(remaining);
        }
    }
}
