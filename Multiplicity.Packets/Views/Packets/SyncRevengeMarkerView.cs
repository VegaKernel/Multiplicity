using System;
using System.Buffers.Binary;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SyncRevengeMarkerView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public SyncRevengeMarkerView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.SyncRevengeMarker);
                PacketReader reader = packet.CreatePayloadReader();
                int uniqueId = reader.ReadInt32();
                float locationX = reader.ReadSingle();
                float locationY = reader.ReadSingle();
                int npcNetId = reader.ReadInt32();
                float npcHpPercent = reader.ReadSingle();
                int npcTypeAgainstDiscouragement = reader.ReadInt32();
                int npcAiStyleAgainstDiscouragement = reader.ReadInt32();
                int coinsValue = reader.ReadInt32();
                float baseValue = reader.ReadSingle();
                bool spawnedFromStatue = reader.ReadBoolean();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                UniqueId = uniqueId;
                LocationX = locationX;
                LocationY = locationY;
                NpcNetId = npcNetId;
                NpcHpPercent = npcHpPercent;
                NpcTypeAgainstDiscouragement = npcTypeAgainstDiscouragement;
                NpcAiStyleAgainstDiscouragement = npcAiStyleAgainstDiscouragement;
                CoinsValue = coinsValue;
                BaseValue = baseValue;
                SpawnedFromStatue = spawnedFromStatue;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public int UniqueId { get; }
            public float LocationX { get; }
            public float LocationY { get; }
            public int NpcNetId { get; }
            public float NpcHpPercent { get; }
            public int NpcTypeAgainstDiscouragement { get; }
            public int NpcAiStyleAgainstDiscouragement { get; }
            public int CoinsValue { get; }
            public float BaseValue { get; }
            public bool SpawnedFromStatue { get; }
        }
}

