using System;
using System.IO;
using System.Text;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct StringPacketView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public StringPacketView(PacketTypes packetType, ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(packetType, payload))
        {
        }

        public static StringPacketView FromPayload(PacketTypes packetType, ReadOnlySpan<byte> payload)
        {
            return new StringPacketView(packetType, payload);
        }

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
            _sourceSpan = packet.SourceSpan;
            ValueUtf8 = valueUtf8;
        }

        public PacketHeader Header { get; }

        public PacketTypes PacketType => Header.PacketType;

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;

        public ReadOnlySpan<byte> ValueUtf8 { get; }

        public string GetValue()
        {
            return Encoding.UTF8.GetString(ValueUtf8);
        }

        public static bool IsSupported(PacketTypes packetType)
        {
            return packetType == PacketTypes.ConnectRequest
                || packetType == PacketTypes.Disconnect
                || packetType == PacketTypes.PasswordSend
                || packetType == PacketTypes.ClientUUID
                || packetType == PacketTypes.HostToken;
        }
    }
}




