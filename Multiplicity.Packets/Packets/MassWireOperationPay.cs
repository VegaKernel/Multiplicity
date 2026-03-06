using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The MassWireOperationPay (0x6E) packet.
    /// </summary>
    public class MassWireOperationPay : TerrariaPacket
    {
        private const short PayloadLength = 5;

        public short ItemType { get; set; }
        public short Quantity { get; set; }
        public byte PlayerId { get; set; }

        public MassWireOperationPay()
            : base((byte)PacketTypes.MassWireOperationPay)
        {
        }

        public MassWireOperationPay(BinaryReader br)
            : base(br)
        {
            ItemType = br.ReadInt16();
            Quantity = br.ReadInt16();
            PlayerId = br.ReadByte();
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
                bw.Write(PlayerId);
            }
        }

        public override string ToString()
        {
            return $"[MassWireOperationPay: ItemType = {ItemType}, Quantity = {Quantity}, PlayerId = {PlayerId}]";
        }
    }
}

