using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The UniqueTownNPCInfoSyncRequest / UpdateNPCName (56) packet.
    /// Client requests contain only NpcId; server responses append Name and TownNpcVariationIndex.
    /// </summary>
    public class UpdateNPCName : TerrariaPacket
    {
        private const short FixedPayloadLength = 2;
        private const short VariationPayloadLength = 4;

        public short NpcId { get; set; }
        public string Name { get; set; }
        public int TownNpcVariationIndex { get; set; }
        public bool HasNameData { get; set; }

        public UpdateNPCName()
            : base((byte)PacketTypes.UpdateNPCName)
        {
            Name = string.Empty;
        }

        public UpdateNPCName(BinaryReader br)
            : base(br)
        {
            NpcId = br.ReadInt16();
            Name = string.Empty;

            if (br.BaseStream.Position < br.BaseStream.Length)
            {
                HasNameData = true;
                Name = br.ReadString();
                TownNpcVariationIndex = br.ReadInt32();
            }
        }

        public override short GetLength()
        {
            if (!ShouldWriteNameData())
            {
                return FixedPayloadLength;
            }

            return checked((short)(FixedPayloadLength +
                PacketSerializationHelper.GetSerializedStringLength(Name) +
                VariationPayloadLength));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            bw.Write(NpcId);
            if (ShouldWriteNameData())
            {
                bw.Write(Name ?? string.Empty);
                bw.Write(TownNpcVariationIndex);
            }
        }

        public override string ToString()
        {
            return $"[UpdateNPCName: NpcId = {NpcId} HasNameData = {HasNameData} Name = {Name} TownNpcVariationIndex = {TownNpcVariationIndex}]";
        }

        private bool ShouldWriteNameData()
        {
            return HasNameData || !string.IsNullOrEmpty(Name) || TownNpcVariationIndex != 0;
        }
    }
}
