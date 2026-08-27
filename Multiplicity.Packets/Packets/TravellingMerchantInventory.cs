using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The TravellingMerchantInventory (0x48) packet.
    /// </summary>
    public class TravellingMerchantInventory : TerrariaPacket
    {
        private const int SlotCount = 40;
        private const short PayloadLength = SlotCount * sizeof(short);

        public short[] Items { get; set; } = new short[SlotCount];

        public TravellingMerchantInventory()
            : base((byte)PacketTypes.TravellingMerchantInventory)
        {
        }

        public TravellingMerchantInventory(BinaryReader br)
            : base(br)
        {
            Items = new short[SlotCount];
            for (int i = 0; i < SlotCount; i++)
            {
                Items[i] = br.ReadInt16();
            }
        }

        public override string ToString()
        {
            return "[TravellingMerchantInventory]";
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

            if (Items == null || Items.Length != SlotCount)
            {
                throw new InvalidDataException($"TravellingMerchantInventory requires exactly {SlotCount} item slots.");
            }

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            for (int i = 0; i < SlotCount; i++)
            {
                bw.Write(Items[i]);
            }
        }
    }
}
