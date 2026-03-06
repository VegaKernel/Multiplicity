using System;
using System.IO;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PacketView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;
        private readonly ReadOnlySpan<byte> _payloadSpan;
        private readonly bool _hasPacketSpan;

        public PacketView(PacketHeader header, ReadOnlySpan<byte> packetSpan)
        {
            if (packetSpan.Length != header.PacketLength)
            {
                throw new InvalidDataException($"Packet span length {packetSpan.Length} does not match header length {header.PacketLength}.");
            }

            Header = header;
            _packetSpan = packetSpan;
            _payloadSpan = packetSpan.Slice(PacketHeader.Size, header.PayloadLength);
            _hasPacketSpan = true;
        }

        private PacketView(PacketHeader header, ReadOnlySpan<byte> packetSpan, ReadOnlySpan<byte> payloadSpan, bool hasPacketSpan)
        {
            if (payloadSpan.Length != header.PayloadLength)
            {
                throw new InvalidDataException($"Payload span length {payloadSpan.Length} does not match header payload length {header.PayloadLength}.");
            }

            Header = header;
            _packetSpan = packetSpan;
            _payloadSpan = payloadSpan;
            _hasPacketSpan = hasPacketSpan;
        }

        public static PacketView FromPayload(PacketTypes packetType, ReadOnlySpan<byte> payloadSpan)
        {
            return new PacketView(PacketHeader.FromPayload(packetType, payloadSpan.Length), default, payloadSpan, false);
        }

        public static PacketView FromPayload(byte rawPacketId, ReadOnlySpan<byte> payloadSpan)
        {
            return new PacketView(PacketHeader.FromPayload(rawPacketId, payloadSpan.Length), default, payloadSpan, false);
        }

        public PacketHeader Header { get; }

        public PacketTypes PacketType => Header.PacketType;

        public ReadOnlySpan<byte> SourceSpan => _hasPacketSpan ? _packetSpan : _payloadSpan;

        public ReadOnlySpan<byte> PacketSpan => GetPacketSpan();

        public ReadOnlySpan<byte> PayloadSpan => _payloadSpan;

        public bool HasPacketSpan => _hasPacketSpan;

        public bool TryGetPacketSpan(out ReadOnlySpan<byte> packetSpan)
        {
            if (_hasPacketSpan)
            {
                packetSpan = _packetSpan;
                return true;
            }

            packetSpan = default;
            return false;
        }

        public ReadOnlySpan<byte> GetPacketSpan()
        {
            if (!_hasPacketSpan)
            {
                throw new InvalidDataException("PacketView does not contain a full packet span. Only the payload span is available.");
            }

            return _packetSpan;
        }

        public PacketReader CreatePayloadReader()
        {
            return new PacketReader(PayloadSpan);
        }

        public void EnsureType(PacketTypes expectedType)
        {
            if (PacketType != expectedType)
            {
                throw new InvalidDataException($"Expected packet type {expectedType}, got {PacketType}.");
            }
        }
    }
}
