using System;
using System.Text;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct Utf8StringPacketView
    {
        private readonly ReadOnlySpan<byte> _utf8Bytes;

        public Utf8StringPacketView(in PacketView packet, PacketTypes expectedPacketType)
        {
            packet.EnsureType(expectedPacketType);
            PacketType = expectedPacketType;

            PacketReader reader = packet.CreatePayloadReader();
            _utf8Bytes = reader.ReadLengthPrefixedBytes();
            reader.EnsureEnd();
        }

        public PacketTypes PacketType { get; }

        public ReadOnlySpan<byte> Utf8Bytes => _utf8Bytes;

        public string GetString()
        {
            return Encoding.UTF8.GetString(_utf8Bytes);
        }
    }
}
