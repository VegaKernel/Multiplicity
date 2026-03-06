using System.IO;

namespace Multiplicity.Packets.Models
{
    public sealed class ItemSyncData
    {
        public const short PayloadLength = 5;

        public ushort ItemType { get; set; }

        public ushort Stack { get; set; }

        public byte Prefix { get; set; }

        public ItemSyncData()
        {
        }

        public ItemSyncData(BinaryReader br)
        {
            ItemType = br.ReadUInt16();
            Stack = br.ReadUInt16();
            Prefix = br.ReadByte();
        }

        public void ToStream(BinaryWriter bw)
        {
            bw.Write(ItemType);
            bw.Write(Stack);
            bw.Write(Prefix);
        }
    }
}
