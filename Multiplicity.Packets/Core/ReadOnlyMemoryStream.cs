using System;
using System.IO;

namespace Multiplicity.Packets
{
    internal sealed class ReadOnlyMemoryStream : Stream
    {
        private readonly ReadOnlyMemory<byte> _buffer;
        private int _position;

        public ReadOnlyMemoryStream(ReadOnlyMemory<byte> buffer)
        {
            _buffer = buffer;
        }

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length => _buffer.Length;

        public override long Position
        {
            get => _position;
            set
            {
                if (value < 0 || value > _buffer.Length)
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

            if (_position + count > _buffer.Length)
            {
                throw new EndOfStreamException();
            }

            ReadOnlyMemory<byte> segment = _buffer.Slice(_position, count);
            _position += count;
            return segment;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return Read(buffer.AsSpan(offset, count));
        }

        public override int Read(Span<byte> buffer)
        {
            int remaining = _buffer.Length - _position;
            if (remaining <= 0)
            {
                return 0;
            }

            int toRead = Math.Min(buffer.Length, remaining);
            _buffer.Span.Slice(_position, toRead).CopyTo(buffer);
            _position += toRead;
            return toRead;
        }

        public override int ReadByte()
        {
            if (_position >= _buffer.Length)
            {
                return -1;
            }

            return _buffer.Span[_position++];
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            long target = origin switch
            {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => _position + offset,
                SeekOrigin.End => _buffer.Length + offset,
                _ => throw new ArgumentOutOfRangeException(nameof(origin))
            };

            if (target < 0 || target > _buffer.Length)
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
    }
}
