using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ProjectileUpdateView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public ProjectileUpdateView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.ProjectileUpdate);

            PacketReader reader = packet.CreatePayloadReader();
            short projectileId = reader.ReadInt16();
            float positionX = reader.ReadSingle();
            float positionY = reader.ReadSingle();
            float velocityX = reader.ReadSingle();
            float velocityY = reader.ReadSingle();
            byte owner = reader.ReadByte();
            short type = reader.ReadInt16();
            ProjectileUpdateFlags flags = (ProjectileUpdateFlags)reader.ReadByte();
            ProjectileUpdateExtraFlags extraFlags = (flags & ProjectileUpdateFlags.HasExtraFlags) != 0
                ? (ProjectileUpdateExtraFlags)reader.ReadByte()
                : ProjectileUpdateExtraFlags.None;
            float ai0 = 0f;
            float ai1 = 0f;
            ushort bannerIdToRespondTo = 0;
            short damage = 0;
            float knockBack = 0f;
            short originalDamage = 0;
            short projectileUuid = -1;
            float ai2 = 0f;

            if ((flags & ProjectileUpdateFlags.HasAI0) != 0)
            {
                ai0 = reader.ReadSingle();
            }

            if ((flags & ProjectileUpdateFlags.HasAI1) != 0)
            {
                ai1 = reader.ReadSingle();
            }

            if ((flags & ProjectileUpdateFlags.HasBannerIdToRespondTo) != 0)
            {
                bannerIdToRespondTo = reader.ReadUInt16();
            }

            if ((flags & ProjectileUpdateFlags.HasDamage) != 0)
            {
                damage = reader.ReadInt16();
            }

            if ((flags & ProjectileUpdateFlags.HasKnockBack) != 0)
            {
                knockBack = reader.ReadSingle();
            }

            if ((flags & ProjectileUpdateFlags.HasOriginalDamage) != 0)
            {
                originalDamage = reader.ReadInt16();
            }

            if ((flags & ProjectileUpdateFlags.HasProjectileUuid) != 0)
            {
                projectileUuid = reader.ReadInt16();
            }

            if ((extraFlags & ProjectileUpdateExtraFlags.HasAI2) != 0)
            {
                ai2 = reader.ReadSingle();
            }

            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            ProjectileId = projectileId;
            PositionX = positionX;
            PositionY = positionY;
            VelocityX = velocityX;
            VelocityY = velocityY;
            Owner = owner;
            Type = type;
            Flags = flags;
            ExtraFlags = extraFlags;
            AI0 = ai0;
            AI1 = ai1;
            BannerIdToRespondTo = bannerIdToRespondTo;
            Damage = damage;
            KnockBack = knockBack;
            OriginalDamage = originalDamage;
            ProjectileUuid = projectileUuid;
            AI2 = ai2;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public short ProjectileId { get; }

        public float PositionX { get; }

        public float PositionY { get; }

        public float VelocityX { get; }

        public float VelocityY { get; }

        public byte Owner { get; }

        public short Type { get; }

        public ProjectileUpdateFlags Flags { get; }

        public ProjectileUpdateExtraFlags ExtraFlags { get; }

        public float AI0 { get; }

        public float AI1 { get; }

        public ushort BannerIdToRespondTo { get; }

        public short Damage { get; }

        public float KnockBack { get; }

        public short OriginalDamage { get; }

        public short ProjectileUuid { get; }

        public float AI2 { get; }

        public bool HasAI0 => (Flags & ProjectileUpdateFlags.HasAI0) != 0;

        public bool HasAI1 => (Flags & ProjectileUpdateFlags.HasAI1) != 0;

        public bool HasBannerIdToRespondTo => (Flags & ProjectileUpdateFlags.HasBannerIdToRespondTo) != 0;

        public bool HasDamage => (Flags & ProjectileUpdateFlags.HasDamage) != 0;

        public bool HasKnockBack => (Flags & ProjectileUpdateFlags.HasKnockBack) != 0;

        public bool HasOriginalDamage => (Flags & ProjectileUpdateFlags.HasOriginalDamage) != 0;

        public bool HasProjectileUuid => (Flags & ProjectileUpdateFlags.HasProjectileUuid) != 0;

        public bool HasAI2 => (ExtraFlags & ProjectileUpdateExtraFlags.HasAI2) != 0;
    }
}
