using System;
using System.Buffers;
using System.IO;
using Multiplicity.Packets;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public sealed class PacketBufferApiTests
    {
        [Fact]
        public void WriteTo_BufferWriter_PublishesExactFrame()
        {
            var packet = new ConnectRequest { Version = "Terraria326" };
            byte[] expected = packet.ToArray();
            var writer = new ArrayBufferWriter<byte>();

            packet.WriteTo(writer);

            Assert.Equal(expected.Length, writer.WrittenCount);
            Assert.Equal(expected, writer.WrittenSpan.ToArray());
        }

        [Fact]
        public void TryWriteTo_UnderWrite_DoesNotAdvanceWriter()
        {
            var packet = new BrokenLengthPacket(writeExtraByte: false);
            var writer = new ArrayBufferWriter<byte>();

            bool written = packet.TryWriteTo(writer, out int bytesWritten);

            Assert.False(written);
            Assert.Equal(0, bytesWritten);
            Assert.Equal(0, writer.WrittenCount);
            Assert.False(packet.TrySerialize(out _));
            Assert.Throws<InvalidDataException>(() => packet.ToArray());
        }

        [Fact]
        public void TryWriteTo_OverWrite_DoesNotAdvanceWriter()
        {
            var packet = new BrokenLengthPacket(writeExtraByte: true);
            var writer = new ArrayBufferWriter<byte>();

            bool written = packet.TryWriteTo(writer, out int bytesWritten);

            Assert.False(written);
            Assert.Equal(0, bytesWritten);
            Assert.Equal(0, writer.WrittenCount);
            Assert.False(packet.TrySerialize(out _));
        }

        [Fact]
        public void Deserialize_SegmentedFrame_RoundTrips()
        {
            var original = new ConnectRequest { Version = "Terraria326" };
            byte[] frame = original.ToArray();
            ReadOnlySequence<byte> sequence = CreateSequence(frame, 2);

            bool decoded = TerrariaPacket.TryDeserialize(in sequence, out TerrariaPacket packet);

            Assert.True(decoded);
            var connect = Assert.IsType<ConnectRequest>(packet);
            Assert.Equal(original.Version, connect.Version);
        }

        [Fact]
        public void DeserializePayload_SegmentedPayload_RoundTrips()
        {
            var original = new ConnectRequest { Version = "Terraria326" };
            byte[] payload = original.ToPayloadArray();
            ReadOnlySequence<byte> sequence = CreateSequence(payload, 3);

            bool decoded = TerrariaPacket.TryDeserializePayload(
                PacketTypes.ConnectRequest,
                in sequence,
                out TerrariaPacket packet);

            Assert.True(decoded);
            var connect = Assert.IsType<ConnectRequest>(packet);
            Assert.Equal(original.Version, connect.Version);
        }

        [Fact]
        public void DeserializePayload_OversizedSegmentedInput_FailsWithoutLargeRent()
        {
            byte[] first = new byte[short.MaxValue / 2];
            byte[] second = new byte[short.MaxValue / 2 + 16];
            ReadOnlySequence<byte> sequence = CreateSequence(first, second);

            bool decoded = TerrariaPacket.TryDeserializePayload(
                PacketTypes.ConnectRequest,
                in sequence,
                out _);

            Assert.False(decoded);
        }

        private static ReadOnlySequence<byte> CreateSequence(byte[] bytes, int split)
        {
            return CreateSequence(bytes[..split], bytes[split..]);
        }

        private static ReadOnlySequence<byte> CreateSequence(byte[] firstBytes, byte[] secondBytes)
        {
            var first = new BufferSegment(firstBytes);
            BufferSegment last = first.Append(secondBytes);
            return new ReadOnlySequence<byte>(first, 0, last, last.Memory.Length);
        }

        private sealed class BufferSegment : ReadOnlySequenceSegment<byte>
        {
            public BufferSegment(ReadOnlyMemory<byte> memory)
            {
                Memory = memory;
            }

            public BufferSegment Append(ReadOnlyMemory<byte> memory)
            {
                var segment = new BufferSegment(memory)
                {
                    RunningIndex = RunningIndex + Memory.Length
                };
                Next = segment;
                return segment;
            }
        }

        private sealed class BrokenLengthPacket : TerrariaPacket
        {
            private readonly bool _writeExtraByte;

            public BrokenLengthPacket(bool writeExtraByte)
                : base((byte)PacketTypes.Ping)
            {
                _writeExtraByte = writeExtraByte;
            }

            public override short GetLength() => 1;

            public override void ToStream(Stream stream, bool includeHeader = true)
            {
                if (includeHeader)
                {
                    base.ToStream(stream, includeHeader);
                }

                if (_writeExtraByte)
                {
                    stream.WriteByte(1);
                    stream.WriteByte(2);
                }
            }
        }
    }
}
