using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Multiplicity.Packets
{
    /// <summary>
    /// Exact-capacity write-only stream used to make packet serialization fail closed without a growable staging
    /// buffer. The caller publishes the destination only after <see cref="WrittenCount"/> matches the expected
    /// length and <see cref="Overflowed"/> is false.
    /// </summary>
    internal sealed class FixedMemoryWriteStream : Stream
    {
        private readonly Memory<byte> _buffer;
        private int _writtenCount;
        private bool _overflowed;

        public FixedMemoryWriteStream(Memory<byte> buffer)
        {
            _buffer = buffer;
        }

        public int WrittenCount => _writtenCount;

        public bool Overflowed => _overflowed;

        public override bool CanRead => false;
        public override bool CanSeek => false;
        public override bool CanWrite => true;
        public override long Length => _writtenCount;

        public override long Position
        {
            get => _writtenCount;
            set => throw new NotSupportedException();
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Write(byte[] buffer, int offset, int count)
        {
            Write(buffer.AsSpan(offset, count));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Write(ReadOnlySpan<byte> source)
        {
            if (source.IsEmpty)
            {
                return;
            }

            int offset = _writtenCount;
            _writtenCount = checked(offset + source.Length);
            Span<byte> destination = _buffer.Span;

            if ((uint)offset <= (uint)destination.Length && source.Length <= destination.Length - offset)
            {
                source.CopyTo(destination.Slice(offset));
                return;
            }

            _overflowed = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void WriteByte(byte value)
        {
            int offset = _writtenCount;
            _writtenCount = checked(offset + 1);
            Span<byte> destination = _buffer.Span;

            if ((uint)offset < (uint)destination.Length)
            {
                destination[offset] = value;
                return;
            }

            _overflowed = true;
        }
    }
}
