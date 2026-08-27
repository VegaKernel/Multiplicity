using System;
using System.IO;

namespace Multiplicity.Packets
{
    internal sealed class PacketRegistration
    {
        private readonly Func<TerrariaPacket> _createDefault;

        private PacketRegistration(
            PacketTypes packetType,
            Type packetClrType,
            Func<BinaryReader, TerrariaPacket> deserialize,
            Func<TerrariaPacket> createDefault,
            bool hasFixedPayloadLength)
        {
            PacketType = packetType;
            PacketClrType = packetClrType;
            Deserialize = deserialize;
            _createDefault = createDefault;
            HasFixedPayloadLength = hasFixedPayloadLength;
        }

        public PacketTypes PacketType { get; }

        public Type PacketClrType { get; }

        public Func<BinaryReader, TerrariaPacket> Deserialize { get; }

        public bool HasFixedPayloadLength { get; }

        public PacketPayloadMetadata CreateMetadata()
        {
            TerrariaPacket packet = _createDefault();
            short defaultPayloadLength = packet.GetLength();

            return new PacketPayloadMetadata(
                PacketType,
                PacketClrType,
                defaultPayloadLength,
                HasFixedPayloadLength ? defaultPayloadLength : null);
        }

        public static PacketRegistration Fixed<TPacket>(
            PacketTypes packetType,
            Func<BinaryReader, TPacket> deserialize,
            Func<TPacket> createDefault)
            where TPacket : TerrariaPacket
        {
            return new PacketRegistration(
                packetType,
                typeof(TPacket),
                reader => deserialize(reader),
                () => createDefault(),
                hasFixedPayloadLength: true);
        }

        public static PacketRegistration Variable<TPacket>(
            PacketTypes packetType,
            Func<BinaryReader, TPacket> deserialize,
            Func<TPacket> createDefault)
            where TPacket : TerrariaPacket
        {
            return new PacketRegistration(
                packetType,
                typeof(TPacket),
                reader => deserialize(reader),
                () => createDefault(),
                hasFixedPayloadLength: false);
        }
    }
}
