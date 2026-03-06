using System.IO;

namespace Multiplicity.Packets
{
    public abstract class TileEntityItemSyncPacketBase : TerrariaPacket
    {
        protected const short PayloadLength = 9;

        public short TileX { get; set; }

        public short TileY { get; set; }

        public short ItemType { get; set; }

        public byte Prefix { get; set; }

        public short Stack { get; set; }

        protected TileEntityItemSyncPacketBase(byte id)
            : base(id)
        {
        }

        protected TileEntityItemSyncPacketBase(BinaryReader br)
            : base(br)
        {
            TileX = br.ReadInt16();
            TileY = br.ReadInt16();
            ItemType = br.ReadInt16();
            Prefix = br.ReadByte();
            Stack = br.ReadInt16();
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
                bw.Write(TileX);
                bw.Write(TileY);
                bw.Write(ItemType);
                bw.Write(Prefix);
                bw.Write(Stack);
            }
        }
    }
}
