using System;
using Multiplicity.Packets.Models;

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
            ProjectileKey piggyBankKey = ProjectileKey.FromPacked(reader.ReadInt32());
            short piggyBankProjectileType = reader.ReadInt16();
            ProjectileKey voidLensChestKey = ProjectileKey.FromPacked(reader.ReadInt32());
            short voidLensChestProjectileType = reader.ReadInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            PiggyBankKey = piggyBankKey;
            PiggyBankProjectileType = piggyBankProjectileType;
            VoidLensChestKey = voidLensChestKey;
            VoidLensChestProjectileType = voidLensChestProjectileType;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;

        public byte PlayerId { get; }

        public ProjectileKey PiggyBankKey { get; }

        public short PiggyBankProjectileType { get; }

        public ProjectileKey VoidLensChestKey { get; }

        public short VoidLensChestProjectileType { get; }

        public bool HasPiggyBankProjectile => PiggyBankProjectileType != 0;

        public bool HasVoidLensChestProjectile => VoidLensChestProjectileType != 0;

        [Obsolete("Use PiggyBankKey. Terraria serializes a packed ProjectileKey.")]
        public short PiggyBankOwnerIndex => HasPiggyBankProjectile ? PiggyBankKey.Spawner : (short)-1;

        [Obsolete("Use PiggyBankKey. Terraria serializes projectile generation as part of the packed key.")]
        public short PiggyBankIdentity => checked((short)PiggyBankKey.Index);

        [Obsolete("Use VoidLensChestKey. Terraria serializes a packed ProjectileKey.")]
        public short VoidLensChestOwnerIndex => HasVoidLensChestProjectile ? VoidLensChestKey.Spawner : (short)-1;

        [Obsolete("Use VoidLensChestKey. Terraria serializes projectile generation as part of the packed key.")]
        public short VoidLensChestIdentity => checked((short)VoidLensChestKey.Index);
    }
}
