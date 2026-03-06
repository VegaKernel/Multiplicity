using System;
using System.IO;
using System.Text;

namespace Multiplicity.Packets
{
    public sealed class PacketBinaryReader : BinaryReader
    {
        private readonly ReadOnlyMemoryStream _stream;

        public PacketBinaryReader(ReadOnlyMemory<byte> buffer)
            : this(new ReadOnlyMemoryStream(buffer))
        {
        }

        private PacketBinaryReader(ReadOnlyMemoryStream stream)
            : base(stream, Encoding.UTF8, leaveOpen: true)
        {
            _stream = stream;
        }

        public ReadOnlyMemory<byte> ReadMemory(int count)
        {
            return _stream.ReadMemory(count);
        }

        public ReadOnlyMemory<byte> ReadRemainingMemory()
        {
            int remaining = (int)(_stream.Length - _stream.Position);
            return _stream.ReadMemory(remaining);
        }
    }
}
