using System;
using System.IO;

namespace Multiplicity.Packets
{
    internal sealed class PacketPayloadStream : Stream, IPacketMemorySource
    {
        private readonly ReadOnlyMemory<byte> _payload;
        private readonly byte _packetId;
        private readonly ushort _packetLength;
        private int _position;

        public PacketPayloadStream(byte packetId, ReadOnlyMemory<byte> payload)
        {
            int totalLength = payload.Length + TerrariaPacket.PACKET_HEADER_LEN;
            if (totalLength > short.MaxValue)
            {
                throw new InvalidDataException(
                    $"Payload is too large to be represented as a Terraria packet. Maximum payload size is {short.MaxValue - TerrariaPacket.PACKET_HEADER_LEN} bytes, got {payload.Length}.");
            }

            _packetId = packetId;
            _payload = payload;
            _packetLength = (ushort)totalLength;
        }

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length => _packetLength;

        public override long Position
        {
            get => _position;
            set
            {
                if (value < 0 || value > Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _position = (int)value;
            }
        }

        public override void Flush()
        {
        }

        public ReadOnlyMemory<byte> ReadMemory(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (_position + count > Length)
            {
                throw new EndOfStreamException();
            }

            int payloadOffset = _position - TerrariaPacket.PACKET_HEADER_LEN;
            if (payloadOffset >= 0 && payloadOffset + count <= _payload.Length)
            {
                ReadOnlyMemory<byte> segment = _payload.Slice(payloadOffset, count);
                _position += count;
                return segment;
            }

            if (count == 0)
            {
                return ReadOnlyMemory<byte>.Empty;
            }

            byte[] copy = new byte[count];
            _ = Read(copy, 0, count);
            return copy;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return Read(buffer.AsSpan(offset, count));
        }

        public override int Read(Span<byte> buffer)
        {
            int remaining = (int)Length - _position;
            if (remaining <= 0)
            {
                return 0;
            }

            int toRead = Math.Min(buffer.Length, remaining);
            int written = 0;

            while (written < toRead)
            {
                if (_position < TerrariaPacket.PACKET_HEADER_LEN)
                {
                    buffer[written++] = GetHeaderByte(_position++);
                    continue;
                }

                int payloadOffset = _position - TerrariaPacket.PACKET_HEADER_LEN;
                int payloadAvailable = _payload.Length - payloadOffset;
                int chunkLength = Math.Min(toRead - written, payloadAvailable);
                _payload.Span.Slice(payloadOffset, chunkLength).CopyTo(buffer.Slice(written, chunkLength));
                _position += chunkLength;
                written += chunkLength;
            }

            return written;
        }

        public override int ReadByte()
        {
            if (_position >= Length)
            {
                return -1;
            }

            if (_position < TerrariaPacket.PACKET_HEADER_LEN)
            {
                return GetHeaderByte(_position++);
            }

            return _payload.Span[_position++ - TerrariaPacket.PACKET_HEADER_LEN];
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            long target = origin switch
            {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => _position + offset,
                SeekOrigin.End => Length + offset,
                _ => throw new ArgumentOutOfRangeException(nameof(origin))
            };

            if (target < 0 || target > Length)
            {
                throw new IOException("Attempted to seek outside the bounds of the packet buffer.");
            }

            _position = (int)target;
            return _position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        private byte GetHeaderByte(int index)
        {
            return index switch
            {
                0 => (byte)(_packetLength & 0xFF),
                1 => (byte)(_packetLength >> 8),
                2 => _packetId,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };
        }
    }
}
