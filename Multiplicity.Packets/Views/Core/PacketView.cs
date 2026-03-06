using System;
using System.IO;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PacketView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public PacketView(PacketHeader header, ReadOnlySpan<byte> packetSpan)
        {
            if (packetSpan.Length != header.PacketLength)
            {
                throw new InvalidDataException($"Packet span length {packetSpan.Length} does not match header length {header.PacketLength}.");
            }

            Header = header;
            _packetSpan = packetSpan;
        }

        public PacketHeader Header { get; }

        public PacketTypes PacketType => Header.PacketType;

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public ReadOnlySpan<byte> PayloadSpan => _packetSpan.Slice(PacketHeader.Size, Header.PayloadLength);

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
