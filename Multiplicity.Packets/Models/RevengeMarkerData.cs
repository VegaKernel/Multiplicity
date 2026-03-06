using System.IO;

namespace Multiplicity.Packets.Models
{
    public sealed class RevengeMarkerData
    {
        public const short PayloadLength = 37;

        public int UniqueId { get; set; }

        public float LocationX { get; set; }

        public float LocationY { get; set; }

        public int NpcNetId { get; set; }

        public float NpcHpPercent { get; set; }

        public int NpcTypeAgainstDiscouragement { get; set; }

        public int NpcAiStyleAgainstDiscouragement { get; set; }

        public int CoinsValue { get; set; }

        public float BaseValue { get; set; }

        public bool SpawnedFromStatue { get; set; }

        public RevengeMarkerData()
        {
        }

        public RevengeMarkerData(BinaryReader br)
        {
            UniqueId = br.ReadInt32();
            LocationX = br.ReadSingle();
            LocationY = br.ReadSingle();
            NpcNetId = br.ReadInt32();
            NpcHpPercent = br.ReadSingle();
            NpcTypeAgainstDiscouragement = br.ReadInt32();
            NpcAiStyleAgainstDiscouragement = br.ReadInt32();
            CoinsValue = br.ReadInt32();
            BaseValue = br.ReadSingle();
            SpawnedFromStatue = br.ReadBoolean();
        }

        public void ToStream(BinaryWriter bw)
        {
            bw.Write(UniqueId);
            bw.Write(LocationX);
            bw.Write(LocationY);
            bw.Write(NpcNetId);
            bw.Write(NpcHpPercent);
            bw.Write(NpcTypeAgainstDiscouragement);
            bw.Write(NpcAiStyleAgainstDiscouragement);
            bw.Write(CoinsValue);
            bw.Write(BaseValue);
            bw.Write(SpawnedFromStatue);
        }
    }
}
