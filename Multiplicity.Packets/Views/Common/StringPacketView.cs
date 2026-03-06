using System;
using System.IO;
using System.Text;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct StringPacketView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public StringPacketView(PacketView packet)
        {
            if (!IsSupported(packet.PacketType))
            {
                throw new InvalidDataException($"Packet type {packet.PacketType} is not a supported string packet.");
            }

            PacketReader reader = packet.CreatePayloadReader();
            ReadOnlySpan<byte> valueUtf8 = reader.ReadLengthPrefixedBytes();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            ValueUtf8 = valueUtf8;
        }

        public PacketHeader Header { get; }

        public PacketTypes PacketType => Header.PacketType;

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public ReadOnlySpan<byte> ValueUtf8 { get; }

        public string GetValue()
        {
            return Encoding.UTF8.GetString(ValueUtf8);
        }

        public static bool IsSupported(PacketTypes packetType)
        {
            return packetType == PacketTypes.ConnectRequest
                || packetType == PacketTypes.Disconnect
                || packetType == PacketTypes.SendPassword
                || packetType == PacketTypes.ClientUUID
                || packetType == PacketTypes.HostToken;
        }
    }
}
