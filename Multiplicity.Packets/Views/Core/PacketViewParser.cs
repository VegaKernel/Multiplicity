using System;
using System.Buffers.Binary;
using System.IO;

namespace Multiplicity.Packets.Views
{
    public static class PacketViewParser
    {
        public static bool TryReadHeader(ReadOnlySpan<byte> buffer, out PacketHeader header)
        {
            header = default;

            if (buffer.Length < PacketHeader.Size)
            {
                return false;
            }

            ushort packetLength = BinaryPrimitives.ReadUInt16LittleEndian(buffer);
            if (packetLength < PacketHeader.Size)
            {
                return false;
            }

            header = new PacketHeader(packetLength, buffer[sizeof(ushort)]);
            return true;
        }

        public static bool TryParse(ReadOnlySpan<byte> buffer, out PacketView packetView, out int consumed)
        {
            packetView = default;
            consumed = 0;

            if (!TryReadHeader(buffer, out PacketHeader header))
            {
                return false;
            }

            if (buffer.Length < header.PacketLength)
            {
                return false;
            }

            packetView = new PacketView(header, buffer.Slice(0, header.PacketLength));
            consumed = header.PacketLength;
            return true;
        }

        public static bool TryParseExact(ReadOnlySpan<byte> packetBuffer, out PacketView packetView)
        {
            packetView = default;

            if (!TryParse(packetBuffer, out PacketView parsedView, out int consumed))
            {
                return false;
            }

            if (consumed != packetBuffer.Length)
            {
                return false;
            }

            packetView = parsedView;
            return true;
        }

        public static PacketView ParseExact(ReadOnlySpan<byte> packetBuffer)
        {
            if (!TryParse(packetBuffer, out PacketView packetView, out int consumed))
            {
                throw new InvalidDataException("Buffer does not contain a complete Terraria packet.");
            }

            if (consumed != packetBuffer.Length)
            {
                throw new InvalidDataException($"Buffer length {packetBuffer.Length} does not match packet length {consumed}.");
            }

            return packetView;
        }
    }
}
