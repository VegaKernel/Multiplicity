using System;
using System.Buffers.Binary;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct UpdatePlayerLuckFactorsView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public UpdatePlayerLuckFactorsView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.UpdatePlayerLuckFactors);
                PacketReader reader = packet.CreatePayloadReader();
                byte playerId = reader.ReadByte();
                int ladyBugLuckTimeLeft = reader.ReadInt32();
                float torchLuck = reader.ReadSingle();
                byte luckPotion = reader.ReadByte();
                bool hasGardenGnomeNearby = reader.ReadBoolean();
                bool brokenMirrorBadLuck = reader.ReadBoolean();
                float equipmentBasedLuckBonus = reader.ReadSingle();
                float coinLuck = reader.ReadSingle();
                byte kiteLuckLevel = reader.ReadByte();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                PlayerId = playerId;
                LadyBugLuckTimeLeft = ladyBugLuckTimeLeft;
                TorchLuck = torchLuck;
                LuckPotion = luckPotion;
                HasGardenGnomeNearby = hasGardenGnomeNearby;
                BrokenMirrorBadLuck = brokenMirrorBadLuck;
                EquipmentBasedLuckBonus = equipmentBasedLuckBonus;
                CoinLuck = coinLuck;
                KiteLuckLevel = kiteLuckLevel;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public byte PlayerId { get; }
            public int LadyBugLuckTimeLeft { get; }
            public float TorchLuck { get; }
            public byte LuckPotion { get; }
            public bool HasGardenGnomeNearby { get; }
            public bool BrokenMirrorBadLuck { get; }
            public float EquipmentBasedLuckBonus { get; }
            public float CoinLuck { get; }
            public byte KiteLuckLevel { get; }
        }
}

