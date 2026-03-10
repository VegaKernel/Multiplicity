using System;

namespace Multiplicity.Packets
{
    public readonly struct PacketPayloadMetadata
    {
        public PacketPayloadMetadata(PacketTypes packetType, Type packetClrType, short defaultPayloadLength, short? fixedPayloadLength)
        {
            PacketType = packetType;
            PacketClrType = packetClrType ?? throw new ArgumentNullException(nameof(packetClrType));
            DefaultPayloadLength = defaultPayloadLength;
            FixedPayloadLength = fixedPayloadLength;
        }

        public PacketTypes PacketType { get; }

        public Type PacketClrType { get; }

        public short DefaultPayloadLength { get; }

        public short? FixedPayloadLength { get; }

        public bool HasFixedPayloadLength => FixedPayloadLength.HasValue;
    }
}
