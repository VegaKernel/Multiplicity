using System;
using System.Buffers.Binary;
using System.IO;

namespace Multiplicity.Packets.Extensions
{
    /// <summary>
    /// Decodes extension overlays before falling back to the vanilla Terraria packet decoder.
    /// The default TerrariaPacket.Deserialize APIs remain vanilla/legacy-compatible.
    /// </summary>
    public sealed class PacketDecoder
    {
        private readonly PacketExtensionRegistry _extensions;

        public PacketDecoder(PacketExtensionRegistry extensions)
        {
            _extensions = extensions ?? throw new ArgumentNullException(nameof(extensions));
        }

        public static PacketDecoder Vanilla { get; } = new PacketDecoder(PacketExtensionRegistry.Empty);

        public static PacketDecoder CommonExtensions { get; } = new PacketDecoder(PacketExtensions.Common);

        public TerrariaPacket Decode(ReadOnlyMemory<byte> packetBuffer)
        {
            ValidatePacketBuffer(packetBuffer);
            byte packetId = packetBuffer.Span[2];

            using PacketBinaryReader reader = new PacketBinaryReader(packetBuffer);
            if (_extensions.TryDeserialize(reader, packetId, out TerrariaPacket extensionPacket))
            {
                return extensionPacket;
            }

            return TerrariaPacket.Deserialize(packetBuffer);
        }

        public TerrariaPacket DecodePayload(byte packetId, ReadOnlyMemory<byte> payload)
        {
            ValidatePayload(payload);

            using PacketBinaryReader reader = new PacketBinaryReader(packetId, payload);
            if (_extensions.TryDeserialize(reader, packetId, out TerrariaPacket extensionPacket))
            {
                return extensionPacket;
            }

            return TerrariaPacket.DeserializePayload(packetId, payload);
        }

        public TerrariaPacket DecodePayload(PacketTypes packetType, ReadOnlyMemory<byte> payload)
        {
            return DecodePayload((byte)packetType, payload);
        }

        public bool TryDecode(ReadOnlyMemory<byte> packetBuffer, out TerrariaPacket packet)
        {
            try
            {
                packet = Decode(packetBuffer);
                return true;
            }
            catch (InvalidDataException)
            {
                packet = null;
                return false;
            }
            catch (EndOfStreamException)
            {
                packet = null;
                return false;
            }
        }

        public bool TryDecodePayload(byte packetId, ReadOnlyMemory<byte> payload, out TerrariaPacket packet)
        {
            try
            {
                packet = DecodePayload(packetId, payload);
                return true;
            }
            catch (InvalidDataException)
            {
                packet = null;
                return false;
            }
            catch (EndOfStreamException)
            {
                packet = null;
                return false;
            }
        }

        private static void ValidatePacketBuffer(ReadOnlyMemory<byte> packetBuffer)
        {
            if (packetBuffer.Length < TerrariaPacket.PacketHeaderLength)
            {
                throw new InvalidDataException(
                    $"Packet buffer is too short. Expected at least {TerrariaPacket.PacketHeaderLength} bytes, got {packetBuffer.Length}.");
            }

            short declaredLength = BinaryPrimitives.ReadInt16LittleEndian(packetBuffer.Span);
            if (declaredLength < TerrariaPacket.PacketHeaderLength || declaredLength != packetBuffer.Length)
            {
                throw new InvalidDataException(
                    $"Packet buffer length mismatch. Header says {declaredLength} bytes, actual slice is {packetBuffer.Length} bytes.");
            }
        }

        private static void ValidatePayload(ReadOnlyMemory<byte> payload)
        {
            if (payload.Length > short.MaxValue - TerrariaPacket.PacketHeaderLength)
            {
                throw new InvalidDataException(
                    $"Payload buffer is too large. Maximum payload size is {short.MaxValue - TerrariaPacket.PacketHeaderLength} bytes, got {payload.Length}.");
            }
        }
    }
}
