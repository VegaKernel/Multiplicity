using System.IO;

namespace Multiplicity.Packets.Models
{
    public sealed class LiquidUpdate
    {
        public int X { get; set; }

        public int Y { get; set; }

        public byte Liquid { get; set; }

        public LiquidKind LiquidType { get; set; }

        public LiquidUpdate(BinaryReader br)
        {
            int packedCoordinates = br.ReadInt32();
            X = (packedCoordinates >> 16) & ushort.MaxValue;
            Y = packedCoordinates & ushort.MaxValue;
            Liquid = br.ReadByte();
            LiquidType = (LiquidKind)br.ReadByte();
        }

        public void ToStream(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                int packedCoordinates = (X << 16) | Y;
                bw.Write(packedCoordinates);
                bw.Write(Liquid);
                bw.Write((byte)LiquidType);
            }
        }
    }
}
