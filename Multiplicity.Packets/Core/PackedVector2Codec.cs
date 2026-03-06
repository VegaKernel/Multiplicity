using System;
using System.Numerics;

namespace Multiplicity.Packets
{
    internal static class PackedVector2Codec
    {
        public static uint Pack(Vector2 value)
        {
            uint x = BitConverter.HalfToUInt16Bits((Half)value.X);
            uint y = BitConverter.HalfToUInt16Bits((Half)value.Y);
            return x | (y << 16);
        }

        public static Vector2 Unpack(uint packedValue)
        {
            Half x = BitConverter.UInt16BitsToHalf((ushort)(packedValue & 0xFFFF));
            Half y = BitConverter.UInt16BitsToHalf((ushort)(packedValue >> 16));
            return new Vector2((float)x, (float)y);
        }
    }
}
