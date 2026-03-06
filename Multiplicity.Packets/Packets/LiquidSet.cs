using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The LiquidSet (0x30) packet.
    /// </summary>
    public class LiquidSet : TerrariaPacket
    {
        private const short PayloadLength = 6;

        public short X { get; set; }
        public short Y { get; set; }
        public byte Liquid { get; set; }
        public LiquidKind LiquidType { get; set; }

        public LiquidSet()
            : base((byte)PacketTypes.LiquidSet)
        {
        }

        public LiquidSet(BinaryReader br)
            : base(br)
        {
            X = br.ReadInt16();
            Y = br.ReadInt16();
            Liquid = br.ReadByte();
            LiquidType = (LiquidKind)br.ReadByte();
        }

        public override short GetLength()
        {
            return PayloadLength;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(X);
                bw.Write(Y);
                bw.Write(Liquid);
                bw.Write((byte)LiquidType);
            }
        }

        public override string ToString()
        {
            return $"[LiquidSet: X = {X}, Y = {Y}, Liquid = {Liquid}, LiquidType = {LiquidType}]";
        }
    }
}
