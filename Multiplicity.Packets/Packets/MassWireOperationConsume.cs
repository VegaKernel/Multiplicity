using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The MassWireOperationConsume (0x6E) packet.
    /// </summary>
    public class MassWireOperationConsume : TerrariaPacket
    {
        private const short PayloadLength = 5;

        public short ItemType { get; set; }
        public short Quantity { get; set; }
        public byte PlayerID { get; set; }

        public MassWireOperationConsume()
            : base((byte)PacketTypes.MassWireOperationConsume)
        {
        }

        public MassWireOperationConsume(BinaryReader br)
            : base(br)
        {
            ItemType = br.ReadInt16();
            Quantity = br.ReadInt16();
            PlayerID = br.ReadByte();
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
                bw.Write(ItemType);
                bw.Write(Quantity);
                bw.Write(PlayerID);
            }
        }

        public override string ToString()
        {
            return $"[MassWireOperationConsume: ItemType = {ItemType}, Quantity = {Quantity}, PlayerID = {PlayerID}]";
        }
    }
}
