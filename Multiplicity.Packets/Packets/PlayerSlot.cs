using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlayerSlot (0x05) packet for protocol 317/318.
    /// </summary>
    public class PlayerSlot : TerrariaPacket
    {
        private const short PayloadLength = 9;

        public byte PlayerId { get; set; }
        public short SlotId { get; set; }
        public short Stack { get; set; }
        public byte Prefix { get; set; }
        public short ItemNetId { get; set; }
        public byte ItemFlags { get; set; }

        public PlayerSlot()
            : base((byte)PacketTypes.PlayerSlot)
        {
        }

        public PlayerSlot(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            SlotId = br.ReadInt16();
            Stack = br.ReadInt16();
            Prefix = br.ReadByte();
            ItemNetId = br.ReadInt16();
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
                bw.Write(PlayerId);
                bw.Write(SlotId);
                bw.Write(Stack);
                bw.Write(Prefix);
                bw.Write(ItemNetId);
                bw.Write(ItemFlags);
            }
        }
    }
}

