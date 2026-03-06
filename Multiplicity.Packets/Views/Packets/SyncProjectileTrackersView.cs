using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SyncProjectileTrackersView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public SyncProjectileTrackersView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.SyncProjectileTrackers);
                PacketReader reader = packet.CreatePayloadReader();
                byte playerId = reader.ReadByte();
                short piggyBankOwnerIndex = reader.ReadInt16();
                short piggyBankIdentity = 0;
                short piggyBankProjectileType = 0;
    
                if (piggyBankOwnerIndex != -1)
                {
                    piggyBankIdentity = reader.ReadInt16();
                    piggyBankProjectileType = reader.ReadInt16();
                }
    
                short voidLensChestOwnerIndex = reader.ReadInt16();
                short voidLensChestIdentity = 0;
                short voidLensChestProjectileType = 0;
    
                if (voidLensChestOwnerIndex != -1)
                {
                    voidLensChestIdentity = reader.ReadInt16();
                    voidLensChestProjectileType = reader.ReadInt16();
                }
    
                reader.EnsureEnd();
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                PlayerId = playerId;
                PiggyBankOwnerIndex = piggyBankOwnerIndex;
                PiggyBankIdentity = piggyBankIdentity;
                PiggyBankProjectileType = piggyBankProjectileType;
                VoidLensChestOwnerIndex = voidLensChestOwnerIndex;
                VoidLensChestIdentity = voidLensChestIdentity;
                VoidLensChestProjectileType = voidLensChestProjectileType;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public byte PlayerId { get; }
            public short PiggyBankOwnerIndex { get; }
            public short PiggyBankIdentity { get; }
            public short PiggyBankProjectileType { get; }
            public short VoidLensChestOwnerIndex { get; }
            public short VoidLensChestIdentity { get; }
            public short VoidLensChestProjectileType { get; }
            public bool HasPiggyBankProjectile => PiggyBankOwnerIndex != -1;
            public bool HasVoidLensChestProjectile => VoidLensChestOwnerIndex != -1;
        }
}

