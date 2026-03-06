using System.IO;

namespace Multiplicity.Packets
{
    public abstract class WorldItemSyncPacketBase : TerrariaPacket
    {
        protected const short PayloadLength = 24;

        public short ItemIndex { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public short Stack { get; set; }
        public byte Prefix { get; set; }
        public WorldItemSyncFlags Flags { get; set; }
        public short ItemNetId { get; set; }

        protected WorldItemSyncPacketBase(byte id)
            : base(id)
        {
        }

        protected WorldItemSyncPacketBase(BinaryReader br)
            : base(br)
        {
            ItemIndex = br.ReadInt16();
            PositionX = br.ReadSingle();
            PositionY = br.ReadSingle();
            VelocityX = br.ReadSingle();
            VelocityY = br.ReadSingle();
            Stack = br.ReadInt16();
            Prefix = br.ReadByte();
            Flags = (WorldItemSyncFlags)br.ReadByte();
            ItemNetId = br.ReadInt16();
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
                bw.Write(ItemIndex);
                bw.Write(PositionX);
                bw.Write(PositionY);
                bw.Write(VelocityX);
                bw.Write(VelocityY);
                bw.Write(Stack);
                bw.Write(Prefix);
                bw.Write((byte)Flags);
                bw.Write(ItemNetId);
            }
        }
    }
}

