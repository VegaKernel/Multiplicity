using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The SetNPCShopItem (0x68) packet.
    /// </summary>
    public class SetNPCShopItem : TerrariaPacket
    {
        private const short PayloadLength = 11;

        public byte Slot { get; set; }
        public short ItemType { get; set; }
        public short Stack { get; set; }
        public byte Prefix { get; set; }
        public int Value { get; set; }
        public NpcShopItemFlags Flags { get; set; }

        public SetNPCShopItem()
            : base((byte)PacketTypes.SetNPCShopItem)
        {
        }

        public SetNPCShopItem(BinaryReader br)
            : base(br)
        {
            Slot = br.ReadByte();
            ItemType = br.ReadInt16();
            Stack = br.ReadInt16();
            Prefix = br.ReadByte();
            Value = br.ReadInt32();
            Flags = (NpcShopItemFlags)br.ReadByte();
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
                bw.Write(Slot);
                bw.Write(ItemType);
                bw.Write(Stack);
                bw.Write(Prefix);
                bw.Write(Value);
                bw.Write((byte)Flags);
            }
        }

        public override string ToString()
        {
            return $"[SetNPCShopItem: Slot = {Slot}, ItemType = {ItemType}, Stack = {Stack}, Prefix = {Prefix}, Value = {Value}, Flags = {Flags}]";
        }
    }
}
