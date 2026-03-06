using System.IO;

namespace Multiplicity.Packets
{
    public sealed class SyncChestSize : TerrariaPacket
    {
        private const short PayloadLength = 4;

        public short ChestId { get; set; }
        public short NewSize { get; set; }

        public SyncChestSize()
            : base((byte)PacketTypes.SyncChestSize)
        {
        }

        public SyncChestSize(BinaryReader br)
            : base(br)
        {
            ChestId = br.ReadInt16();
            NewSize = br.ReadInt16();
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
                bw.Write(ChestId);
                bw.Write(NewSize);
            }
        }
    }
}
