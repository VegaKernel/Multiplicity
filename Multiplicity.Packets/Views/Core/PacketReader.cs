using System;
using System.Buffers.Binary;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Multiplicity.Packets.Views
{
    public ref struct PacketReader
    {
        private const int Max7BitEncodedIntBytes = 5;

        private readonly ReadOnlySpan<byte> _buffer;
        private int _position;

        public PacketReader(ReadOnlySpan<byte> buffer)
        {
            _buffer = buffer;
            _position = 0;
        }

        public int Consumed => _position;

        public int Length => _buffer.Length;

        public int RemainingCount => _buffer.Length - _position;

        public bool EndOfBuffer => _position >= _buffer.Length;

        public ReadOnlySpan<byte> RemainingSpan => _buffer.Slice(_position);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadByte()
        {
            EnsureRemaining(sizeof(byte));
            return _buffer[_position++];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte ReadSByte()
        {
            return unchecked((sbyte)ReadByte());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadBoolean()
        {
            return ReadByte() != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ReadInt16()
        {
            return unchecked((short)ReadUInt16());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ReadUInt16()
        {
            ReadOnlySpan<byte> span = ReadSpan(sizeof(ushort));
            return BinaryPrimitives.ReadUInt16LittleEndian(span);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt32()
        {
            ReadOnlySpan<byte> span = ReadSpan(sizeof(int));
            return BinaryPrimitives.ReadInt32LittleEndian(span);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt32()
        {
            ReadOnlySpan<byte> span = ReadSpan(sizeof(uint));
            return BinaryPrimitives.ReadUInt32LittleEndian(span);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadSingle()
        {
            return BitConverter.Int32BitsToSingle(ReadInt32());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 ReadVector2()
        {
            return new Vector2(ReadSingle(), ReadSingle());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 ReadPackedVector2()
        {
            return PackedVector2Codec.Unpack(ReadUInt32());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> ReadSpan(int count)
        {
            EnsureRemaining(count);
            ReadOnlySpan<byte> span = _buffer.Slice(_position, count);
            _position += count;
            return span;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<byte> ReadRemainingSpan()
        {
            ReadOnlySpan<byte> span = RemainingSpan;
            _position = _buffer.Length;
            return span;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Skip(int count)
        {
            EnsureRemaining(count);
            _position += count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnsureRemaining(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (RemainingCount < count)
            {
                throw new InvalidDataException($"Attempted to read {count} bytes with only {RemainingCount} bytes remaining.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnsureEnd()
        {
            if (!EndOfBuffer)
            {
                throw new InvalidDataException($"Packet payload has {RemainingCount} unread bytes.");
            }
        }

        public int Read7BitEncodedInt()
        {
            int value = 0;
            int shift = 0;

            for (int i = 0; i < Max7BitEncodedIntBytes; i++)
            {
                byte current = ReadByte();
                value |= (current & 0x7F) << shift;
                if ((current & 0x80) == 0)
                {
                    return value;
                }

                shift += 7;
            }

            throw new InvalidDataException("7-bit encoded integer is too large.");
        }

        public ReadOnlySpan<byte> ReadLengthPrefixedBytes()
        {
            int length = Read7BitEncodedInt();
            if (length < 0)
            {
                throw new InvalidDataException($"Negative length-prefixed payload: {length}.");
            }

            return ReadSpan(length);
        }

        public string ReadStringUtf8()
        {
            return Encoding.UTF8.GetString(ReadLengthPrefixedBytes());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ColorStruct ReadColor()
        {
            return new ColorStruct
            {
                R = ReadByte(),
                G = ReadByte(),
                B = ReadByte()
            };
        }
    }
}
