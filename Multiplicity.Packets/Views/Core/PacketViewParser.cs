using System;
using System.Buffers.Binary;
using System.IO;
using Multiplicity.Packets;

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

        public static bool TryParsePayload(PacketTypes packetType, ReadOnlySpan<byte> payloadBuffer, out PacketView packetView)
        {
            try
            {
                if (!TryParsePayload(packetType, payloadBuffer, out PacketView parsedView, out int consumed) ||
                    consumed != payloadBuffer.Length)
                {
                    packetView = default;
                    return false;
                }

                packetView = parsedView;
                return true;
            }
            catch (InvalidDataException)
            {
                packetView = default;
                return false;
            }
        }

        public static bool TryParsePayload(byte rawPacketId, ReadOnlySpan<byte> payloadBuffer, out PacketView packetView)
        {
            try
            {
                if (!TryParsePayload(rawPacketId, payloadBuffer, out PacketView parsedView, out int consumed) ||
                    consumed != payloadBuffer.Length)
                {
                    packetView = default;
                    return false;
                }

                packetView = parsedView;
                return true;
            }
            catch (InvalidDataException)
            {
                packetView = default;
                return false;
            }
        }

        public static bool TryParsePayload(PacketTypes packetType, ReadOnlySpan<byte> payloadBuffer, out PacketView packetView, out int consumed)
        {
            if (TerrariaPacket.TryGetFixedPayloadLength(packetType, out short fixedPayloadLength))
            {
                if (payloadBuffer.Length < fixedPayloadLength)
                {
                    packetView = default;
                    consumed = 0;
                    return false;
                }

                packetView = PacketView.FromPayload(packetType, payloadBuffer.Slice(0, fixedPayloadLength));
                consumed = fixedPayloadLength;
                return true;
            }

            ReadOnlyMemory<byte> payloadMemory = payloadBuffer.ToArray();
            return TryParsePayload(packetType, payloadMemory, out packetView, out consumed);
        }

        public static bool TryParsePayload(byte rawPacketId, ReadOnlySpan<byte> payloadBuffer, out PacketView packetView, out int consumed)
        {
            if (TerrariaPacket.TryGetFixedPayloadLength((PacketTypes)rawPacketId, out short fixedPayloadLength))
            {
                if (payloadBuffer.Length < fixedPayloadLength)
                {
                    packetView = default;
                    consumed = 0;
                    return false;
                }

                packetView = PacketView.FromPayload(rawPacketId, payloadBuffer.Slice(0, fixedPayloadLength));
                consumed = fixedPayloadLength;
                return true;
            }

            ReadOnlyMemory<byte> payloadMemory = payloadBuffer.ToArray();
            return TryParsePayload(rawPacketId, payloadMemory, out packetView, out consumed);
        }

        public static bool TryParsePayload(PacketTypes packetType, ReadOnlyMemory<byte> payloadBuffer, out PacketView packetView, out int consumed)
        {
            return TryParsePayload((byte)packetType, payloadBuffer, out packetView, out consumed);
        }

        public static bool TryParsePayload(byte rawPacketId, ReadOnlyMemory<byte> payloadBuffer, out PacketView packetView, out int consumed)
        {
            packetView = default;
            consumed = 0;

            if (TerrariaPacket.TryGetFixedPayloadLength((PacketTypes)rawPacketId, out short fixedPayloadLength))
            {
                if (payloadBuffer.Length < fixedPayloadLength)
                {
                    return false;
                }

                packetView = PacketView.FromPayload(rawPacketId, payloadBuffer.Slice(0, fixedPayloadLength).Span);
                consumed = fixedPayloadLength;
                return true;
            }

            try
            {
                using PacketBinaryReader reader = new PacketBinaryReader(rawPacketId, payloadBuffer);
                _ = TerrariaPacket.Deserialize(reader, rawPacketId);
                int payloadConsumed = checked((int)reader.BaseStream.Position - TerrariaPacket.PacketHeaderLength);
                if (payloadConsumed < 0 || payloadBuffer.Length < payloadConsumed)
                {
                    return false;
                }

                packetView = PacketView.FromPayload(rawPacketId, payloadBuffer.Slice(0, payloadConsumed).Span);
                consumed = payloadConsumed;
                return true;
            }
            catch (InvalidDataException)
            {
                return false;
            }
            catch (EndOfStreamException)
            {
                return false;
            }
        }

        public static bool TryParsePayload(PacketTypes packetType, byte[] buffer, int offset, int length, out PacketView packetView)
        {
            return TryParsePayload(packetType, new ReadOnlySpan<byte>(buffer, offset, length), out packetView);
        }

        public static bool TryParsePayload(byte rawPacketId, byte[] buffer, int offset, int length, out PacketView packetView)
        {
            return TryParsePayload(rawPacketId, new ReadOnlySpan<byte>(buffer, offset, length), out packetView);
        }

        public static bool TryParsePayload(PacketTypes packetType, byte[] buffer, int offset, out PacketView packetView, out int consumed)
        {
            return TryParsePayload(packetType, new ReadOnlyMemory<byte>(buffer, offset, buffer.Length - offset), out packetView, out consumed);
        }

        public static bool TryParsePayload(byte rawPacketId, byte[] buffer, int offset, out PacketView packetView, out int consumed)
        {
            return TryParsePayload(rawPacketId, new ReadOnlyMemory<byte>(buffer, offset, buffer.Length - offset), out packetView, out consumed);
        }

        public static PacketView ParsePayload(PacketTypes packetType, ReadOnlySpan<byte> payloadBuffer)
        {
            return PacketView.FromPayload(packetType, payloadBuffer);
        }

        public static PacketView ParsePayload(byte rawPacketId, ReadOnlySpan<byte> payloadBuffer)
        {
            return PacketView.FromPayload(rawPacketId, payloadBuffer);
        }

        public static PacketView ParsePayload(PacketTypes packetType, byte[] buffer, int offset, int length)
        {
            return ParsePayload(packetType, new ReadOnlySpan<byte>(buffer, offset, length));
        }

        public static PacketView ParsePayload(byte rawPacketId, byte[] buffer, int offset, int length)
        {
            return ParsePayload(rawPacketId, new ReadOnlySpan<byte>(buffer, offset, length));
        }
    }
}
