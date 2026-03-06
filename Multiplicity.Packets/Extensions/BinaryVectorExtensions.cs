using System.IO;
using System.Numerics;

namespace Multiplicity.Packets
{
    internal static class BinaryVectorExtensions
    {
        public static Vector2 ReadVector2(this BinaryReader reader)
        {
            return new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }

        public static Vector2 ReadPackedVector2(this BinaryReader reader)
        {
            return PackedVector2Codec.Unpack(reader.ReadUInt32());
        }

        public static void WriteVector2(this BinaryWriter writer, Vector2 value)
        {
            writer.Write(value.X);
            writer.Write(value.Y);
        }

        public static void WritePackedVector2(this BinaryWriter writer, Vector2 value)
        {
            writer.Write(PackedVector2Codec.Pack(value));
        }
    }
}
