using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// Abstract base class generically representing a Terraria packet.
    /// </summary>
    public abstract class TerrariaPacket : TerrariaNetworkObject
    {
        public const short PacketHeaderLength = 3;

        public byte[] TestRawBuffer { get; set; }

        private static readonly IReadOnlyDictionary<PacketTypes, PacketPayloadMetadata> PayloadMetadataMap =
            VanillaPacketRegistry.CreatePayloadMetadataMap();

        /// <summary>
        /// Vanilla desktop packet deserializers. The map is created from the same explicit,
        /// compile-time registry that supplies packet metadata.
        /// </summary>
        public static Dictionary<PacketTypes, Func<BinaryReader, TerrariaPacket>> DeserializerMap =
            VanillaPacketRegistry.CreateDeserializerMap();

        protected short _length = 0;

        /// <summary>
        /// Gets or sets the packet identifier.
        /// </summary>
        public byte Id { get; protected set; }

        /// <summary>
        /// Gets the packet identifier as a vanilla packet enum value.
        /// Extension packets may use byte ids outside the vanilla desktop range.
        /// </summary>
        public PacketTypes PacketType => (PacketTypes)Id;

        protected TerrariaPacket(BinaryReader br)
        {
            _length = br.ReadInt16();
            Id = br.ReadByte();
        }

        protected TerrariaPacket(byte id)
        {
            Id = id;
        }

        /// <summary>
        /// Deserializes a packet using the vanilla desktop packet registry.
        /// </summary>
        public static TerrariaPacket Deserialize(BinaryReader br, byte id)
        {
            br.BaseStream.Seek(0, SeekOrigin.Begin);

            if (!DeserializerMap.TryGetValue((PacketTypes)id, out Func<BinaryReader, TerrariaPacket> deserializer))
            {
                return new UnknownPacket(br);
            }

            return deserializer(br);
        }

        public static TerrariaPacket Deserialize(ReadOnlyMemory<byte> packetBuffer)
        {
            ValidatePacketBuffer(packetBuffer);
            using PacketBinaryReader br = new PacketBinaryReader(packetBuffer);
            return Deserialize(br);
        }

        public static TerrariaPacket Deserialize(byte[] packetBuffer, int offset, int length)
        {
            return Deserialize(new ReadOnlyMemory<byte>(packetBuffer, offset, length));
        }

        public static TerrariaPacket DeserializePayload(PacketTypes packetType, ReadOnlyMemory<byte> payloadBuffer)
        {
            ValidatePayloadBuffer(payloadBuffer);
            using PacketBinaryReader br = new PacketBinaryReader(packetType, payloadBuffer);
            return Deserialize(br, (byte)packetType);
        }

        public static TerrariaPacket DeserializePayload(byte packetId, ReadOnlyMemory<byte> payloadBuffer)
        {
            ValidatePayloadBuffer(payloadBuffer);
            using PacketBinaryReader br = new PacketBinaryReader(packetId, payloadBuffer);
            return Deserialize(br, packetId);
        }

        public static TerrariaPacket DeserializePayload(PacketTypes packetType, byte[] payloadBuffer, int offset, int length)
        {
            return DeserializePayload(packetType, new ReadOnlyMemory<byte>(payloadBuffer, offset, length));
        }

        public static TerrariaPacket DeserializePayload(byte packetId, byte[] payloadBuffer, int offset, int length)
        {
            return DeserializePayload(packetId, new ReadOnlyMemory<byte>(payloadBuffer, offset, length));
        }

        public static bool TryDeserialize(ReadOnlyMemory<byte> packetBuffer, out TerrariaPacket packet)
        {
            packet = null;

            try
            {
                packet = Deserialize(packetBuffer);
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

        public static bool TryDeserialize(byte[] packetBuffer, int offset, int length, out TerrariaPacket packet)
        {
            return TryDeserialize(new ReadOnlyMemory<byte>(packetBuffer, offset, length), out packet);
        }

        public static bool TryDeserializePayload(PacketTypes packetType, ReadOnlyMemory<byte> payloadBuffer, out TerrariaPacket packet)
        {
            packet = null;

            try
            {
                packet = DeserializePayload(packetType, payloadBuffer);
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

        public static bool TryDeserializePayload(byte packetId, ReadOnlyMemory<byte> payloadBuffer, out TerrariaPacket packet)
        {
            packet = null;

            try
            {
                packet = DeserializePayload(packetId, payloadBuffer);
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

        public static bool TryDeserializePayload(PacketTypes packetType, byte[] payloadBuffer, int offset, int length, out TerrariaPacket packet)
        {
            return TryDeserializePayload(packetType, new ReadOnlyMemory<byte>(payloadBuffer, offset, length), out packet);
        }

        public static bool TryDeserializePayload(byte packetId, byte[] payloadBuffer, int offset, int length, out TerrariaPacket packet)
        {
            return TryDeserializePayload(packetId, new ReadOnlyMemory<byte>(payloadBuffer, offset, length), out packet);
        }

        public static PacketPayloadMetadata GetPayloadMetadata(PacketTypes packetType)
        {
            if (!TryGetPayloadMetadata(packetType, out PacketPayloadMetadata metadata))
            {
                throw new KeyNotFoundException($"Packet metadata for {packetType} is not registered.");
            }

            return metadata;
        }

        public static bool TryGetPayloadMetadata(PacketTypes packetType, out PacketPayloadMetadata metadata)
        {
            return PayloadMetadataMap.TryGetValue(packetType, out metadata);
        }

        public static short GetDefaultPayloadLength(PacketTypes packetType)
        {
            return GetPayloadMetadata(packetType).DefaultPayloadLength;
        }

        public static bool TryGetFixedPayloadLength(PacketTypes packetType, out short payloadLength)
        {
            if (TryGetPayloadMetadata(packetType, out PacketPayloadMetadata metadata) && metadata.FixedPayloadLength.HasValue)
            {
                payloadLength = metadata.FixedPayloadLength.Value;
                return true;
            }

            payloadLength = 0;
            return false;
        }

        /// <summary>
        /// Deserializes a framed Terraria packet from the supplied reader.
        /// </summary>
        public static TerrariaPacket Deserialize(BinaryReader br)
        {
            br.ReadInt16();
            byte id = br.ReadByte();
            return Deserialize(br, id);
        }

        private static void ValidatePacketBuffer(ReadOnlyMemory<byte> packetBuffer)
        {
            if (packetBuffer.Length < PacketHeaderLength)
            {
                throw new InvalidDataException(
                    $"Packet buffer is too short. Expected at least {PacketHeaderLength} bytes, got {packetBuffer.Length}.");
            }

            short declaredLength = BinaryPrimitives.ReadInt16LittleEndian(packetBuffer.Span);
            if (declaredLength < PacketHeaderLength)
            {
                throw new InvalidDataException(
                    $"Invalid packet header length {declaredLength}. Minimum valid length is {PacketHeaderLength}.");
            }

            if (declaredLength != packetBuffer.Length)
            {
                throw new InvalidDataException(
                    $"Packet buffer length mismatch. Header says {declaredLength} bytes, actual slice is {packetBuffer.Length} bytes.");
            }
        }

        private static void ValidatePayloadBuffer(ReadOnlyMemory<byte> payloadBuffer)
        {
            if (payloadBuffer.Length > short.MaxValue - PacketHeaderLength)
            {
                throw new InvalidDataException(
                    $"Payload buffer is too large. Maximum payload size is {short.MaxValue - PacketHeaderLength} bytes, got {payloadBuffer.Length}.");
            }
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (!includeHeader)
            {
                return;
            }

            using BinaryWriter br = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            br.Write((short)(GetLength() + PacketHeaderLength));
            br.Write(Id);
        }
    }
}
