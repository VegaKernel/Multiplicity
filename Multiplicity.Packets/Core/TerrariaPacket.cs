using System;
using System.Buffers;
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

        private static readonly Lazy<IReadOnlyDictionary<PacketTypes, PacketPayloadMetadata>> PayloadMetadataMap =
            new Lazy<IReadOnlyDictionary<PacketTypes, PacketPayloadMetadata>>(
                VanillaPacketRegistry.CreatePayloadMetadataMap,
                isThreadSafe: true);

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

        /// <summary>
        /// Deserializes a framed packet from a potentially segmented buffer. Single-segment input is borrowed
        /// directly; multi-segment input is copied through a bounded ArrayPool lease.
        /// </summary>
        public static TerrariaPacket Deserialize(in ReadOnlySequence<byte> packetBuffer)
        {
            using SequenceMemoryLease lease = SequenceMemoryLease.Create(
                in packetBuffer,
                short.MaxValue,
                "packet");
            return Deserialize(lease.Memory);
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

        /// <summary>
        /// Deserializes a payload from a potentially segmented buffer without imposing a networking dependency.
        /// </summary>
        public static TerrariaPacket DeserializePayload(PacketTypes packetType, in ReadOnlySequence<byte> payloadBuffer)
        {
            return DeserializePayload((byte)packetType, in payloadBuffer);
        }

        /// <summary>
        /// Deserializes a payload from a potentially segmented buffer without imposing a networking dependency.
        /// </summary>
        public static TerrariaPacket DeserializePayload(byte packetId, in ReadOnlySequence<byte> payloadBuffer)
        {
            using SequenceMemoryLease lease = SequenceMemoryLease.Create(
                in payloadBuffer,
                short.MaxValue - PacketHeaderLength,
                "payload");
            return DeserializePayload(packetId, lease.Memory);
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
            catch (Exception exception) when (IsExpectedDecodeFailure(exception))
            {
                return false;
            }
        }

        public static bool TryDeserialize(byte[] packetBuffer, int offset, int length, out TerrariaPacket packet)
        {
            return TryDeserialize(new ReadOnlyMemory<byte>(packetBuffer, offset, length), out packet);
        }

        public static bool TryDeserialize(in ReadOnlySequence<byte> packetBuffer, out TerrariaPacket packet)
        {
            packet = null;

            try
            {
                packet = Deserialize(in packetBuffer);
                return true;
            }
            catch (Exception exception) when (IsExpectedDecodeFailure(exception))
            {
                return false;
            }
        }

        public static bool TryDeserializePayload(PacketTypes packetType, ReadOnlyMemory<byte> payloadBuffer, out TerrariaPacket packet)
        {
            return TryDeserializePayload((byte)packetType, payloadBuffer, out packet);
        }

        public static bool TryDeserializePayload(byte packetId, ReadOnlyMemory<byte> payloadBuffer, out TerrariaPacket packet)
        {
            packet = null;

            try
            {
                packet = DeserializePayload(packetId, payloadBuffer);
                return true;
            }
            catch (Exception exception) when (IsExpectedDecodeFailure(exception))
            {
                return false;
            }
        }

        public static bool TryDeserializePayload(PacketTypes packetType, in ReadOnlySequence<byte> payloadBuffer, out TerrariaPacket packet)
        {
            return TryDeserializePayload((byte)packetType, in payloadBuffer, out packet);
        }

        public static bool TryDeserializePayload(byte packetId, in ReadOnlySequence<byte> payloadBuffer, out TerrariaPacket packet)
        {
            packet = null;

            try
            {
                packet = DeserializePayload(packetId, in payloadBuffer);
                return true;
            }
            catch (Exception exception) when (IsExpectedDecodeFailure(exception))
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
            return PayloadMetadataMap.Value.TryGetValue(packetType, out metadata);
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

        /// <summary>
        /// Serializes this packet into an exact-size final array and rejects under-write or over-write.
        /// </summary>
        public override byte[] ToArray(bool includeHeader = true)
        {
            if (!TrySerialize(out byte[] packet, includeHeader))
            {
                throw new InvalidDataException(
                    $"Packet {GetType().Name} produced bytes inconsistent with its declared length {GetLength()}.");
            }

            return packet;
        }

        /// <summary>
        /// Attempts to serialize this packet into an exact-size final array. A candidate is published only when
        /// the serializer writes exactly the declared number of bytes.
        /// </summary>
        public bool TrySerialize(out byte[] packet, bool includeHeader = true)
        {
            if (!TryGetSerializedLength(includeHeader, out int expectedLength))
            {
                packet = Array.Empty<byte>();
                return false;
            }

            byte[] candidate = GC.AllocateUninitializedArray<byte>(expectedLength);
            if (!TryWriteExact(candidate.AsMemory(), expectedLength, includeHeader, out _))
            {
                packet = Array.Empty<byte>();
                return false;
            }

            packet = candidate;
            return true;
        }

        /// <summary>
        /// Writes this packet directly to an <see cref="IBufferWriter{T}"/>. The writer is advanced only after
        /// exact-length serialization succeeds, so malformed serializers cannot publish partial frames.
        /// </summary>
        public void WriteTo(IBufferWriter<byte> writer, bool includeHeader = true)
        {
            ArgumentNullException.ThrowIfNull(writer);

            if (!TryWriteTo(writer, out _, includeHeader))
            {
                throw new InvalidDataException(
                    $"Packet {GetType().Name} produced bytes inconsistent with its declared length {GetLength()}.");
            }
        }

        /// <summary>
        /// Attempts to write this packet directly to an <see cref="IBufferWriter{T}"/>. On failure the writer is
        /// not advanced.
        /// </summary>
        public bool TryWriteTo(IBufferWriter<byte> writer, out int bytesWritten, bool includeHeader = true)
        {
            ArgumentNullException.ThrowIfNull(writer);
            bytesWritten = 0;

            if (!TryGetSerializedLength(includeHeader, out int expectedLength))
            {
                return false;
            }

            Memory<byte> destination = writer.GetMemory(expectedLength);
            if (destination.Length < expectedLength ||
                !TryWriteExact(destination, expectedLength, includeHeader, out int candidateBytesWritten))
            {
                return false;
            }

            writer.Advance(candidateBytesWritten);
            bytesWritten = candidateBytesWritten;
            return true;
        }

        /// <summary>
        /// Writes only this packet's payload to an <see cref="IBufferWriter{T}"/>.
        /// </summary>
        public void WritePayloadTo(IBufferWriter<byte> writer)
        {
            WriteTo(writer, includeHeader: false);
        }

        private bool TryGetSerializedLength(bool includeHeader, out int serializedLength)
        {
            int payloadLength = GetLength();
            int headerLength = includeHeader ? PacketHeaderLength : 0;

            if (payloadLength < 0 || payloadLength > short.MaxValue - headerLength)
            {
                serializedLength = 0;
                return false;
            }

            serializedLength = payloadLength + headerLength;
            return true;
        }

        private bool TryWriteExact(
            Memory<byte> destination,
            int expectedLength,
            bool includeHeader,
            out int bytesWritten)
        {
            bytesWritten = 0;
            if (destination.Length < expectedLength)
            {
                return false;
            }

            using FixedMemoryWriteStream stream = new FixedMemoryWriteStream(destination.Slice(0, expectedLength));
            ToStream(stream, includeHeader);
            if (stream.Overflowed || stream.WrittenCount != expectedLength)
            {
                return false;
            }

            bytesWritten = expectedLength;
            return true;
        }

        private static bool IsExpectedDecodeFailure(Exception exception)
        {
            return exception is InvalidDataException or
                IOException or
                OverflowException or
                FormatException or
                ArgumentException;
        }

        private ref struct SequenceMemoryLease
        {
            private byte[] _rented;

            private SequenceMemoryLease(ReadOnlyMemory<byte> memory, byte[] rented)
            {
                Memory = memory;
                _rented = rented;
            }

            public ReadOnlyMemory<byte> Memory { get; private set; }

            public static SequenceMemoryLease Create(
                in ReadOnlySequence<byte> sequence,
                int maximumLength,
                string bufferKind)
            {
                if (sequence.Length > maximumLength)
                {
                    throw new InvalidDataException(
                        $"The {bufferKind} buffer is too large. Maximum size is {maximumLength} bytes, got {sequence.Length}.");
                }

                if (sequence.IsSingleSegment)
                {
                    return new SequenceMemoryLease(sequence.First, rented: null);
                }

                int length = checked((int)sequence.Length);
                byte[] rented = ArrayPool<byte>.Shared.Rent(Math.Max(length, 1));
                try
                {
                    sequence.CopyTo(rented);
                    return new SequenceMemoryLease(rented.AsMemory(0, length), rented);
                }
                catch
                {
                    ArrayPool<byte>.Shared.Return(rented);
                    throw;
                }
            }

            public void Dispose()
            {
                byte[] rented = _rented;
                _rented = null;
                Memory = default;
                if (rented != null)
                {
                    ArrayPool<byte>.Shared.Return(rented);
                }
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
