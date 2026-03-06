using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlayerInventorySlot (0x05) packet for protocol 317/318.
    /// </summary>
    public class PlayerInventorySlot : TerrariaPacket
    {
        private const short PayloadLength = 9;

        public byte PlayerID { get; set; }
        public short SlotID { get; set; }
        public short Stack { get; set; }
        public byte Prefix { get; set; }
        public short ItemNetID { get; set; }
        public byte ItemFlags { get; set; }

        public PlayerInventorySlot()
            : base((byte)PacketTypes.PlayerInventorySlot)
        {
        }

        public PlayerInventorySlot(BinaryReader br)
            : base(br)
        {
            PlayerID = br.ReadByte();
            SlotID = br.ReadInt16();
            Stack = br.ReadInt16();
            Prefix = br.ReadByte();
            ItemNetID = br.ReadInt16();
            ItemFlags = br.ReadByte();
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
                bw.Write(PlayerID);
                bw.Write(SlotID);
                bw.Write(Stack);
                bw.Write(Prefix);
                bw.Write(ItemNetID);
                bw.Write(ItemFlags);
            }
        }
    }
}
