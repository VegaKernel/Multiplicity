using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ProjectileNewView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public ProjectileNewView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ProjectileNew, payload))
        {
        }

        public static ProjectileNewView FromPayload(ReadOnlySpan<byte> payload) => new ProjectileNewView(payload);

        public ProjectileNewView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.ProjectileNew);

            PacketReader reader = packet.CreatePayloadReader();
            ProjectileKey key = ProjectileKey.FromPacked(reader.ReadInt32());
            float positionX = reader.ReadSingle();
            float positionY = reader.ReadSingle();
            float velocityX = reader.ReadSingle();
            float velocityY = reader.ReadSingle();
            short type = reader.ReadInt16();
            ProjectileNewFlags flags = (ProjectileNewFlags)reader.ReadByte();
            ProjectileNewExtraFlags extraFlags = (flags & ProjectileNewFlags.HasExtraFlags) != 0
                ? (ProjectileNewExtraFlags)reader.ReadByte()
                : ProjectileNewExtraFlags.None;
            float ai0 = 0f;
            float ai1 = 0f;
            ushort bannerIdToRespondTo = 0;
            short damage = 0;
            float knockBack = 0f;
            short originalDamage = 0;
            float ai2 = 0f;

            if ((flags & ProjectileNewFlags.HasAI0) != 0) ai0 = reader.ReadSingle();
            if ((flags & ProjectileNewFlags.HasAI1) != 0) ai1 = reader.ReadSingle();
            if ((flags & ProjectileNewFlags.HasBannerIdToRespondTo) != 0) bannerIdToRespondTo = reader.ReadUInt16();
            if ((flags & ProjectileNewFlags.HasDamage) != 0) damage = reader.ReadInt16();
            if ((flags & ProjectileNewFlags.HasKnockBack) != 0) knockBack = reader.ReadSingle();
            if ((flags & ProjectileNewFlags.HasOriginalDamage) != 0) originalDamage = reader.ReadInt16();
            if ((extraFlags & ProjectileNewExtraFlags.HasAI2) != 0) ai2 = reader.ReadSingle();

            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            Key = key;
            PositionX = positionX;
            PositionY = positionY;
            VelocityX = velocityX;
            VelocityY = velocityY;
            Type = type;
            Flags = flags;
            ExtraFlags = extraFlags;
            AI0 = ai0;
            AI1 = ai1;
            BannerIdToRespondTo = bannerIdToRespondTo;
            Damage = damage;
            KnockBack = knockBack;
            OriginalDamage = originalDamage;
            AI2 = ai2;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public ProjectileKey Key { get; }
        public float PositionX { get; }
        public float PositionY { get; }
        public float VelocityX { get; }
        public float VelocityY { get; }
        public short Type { get; }
        public ProjectileNewFlags Flags { get; }
        public ProjectileNewExtraFlags ExtraFlags { get; }
        public float AI0 { get; }
        public float AI1 { get; }
        public ushort BannerIdToRespondTo { get; }
        public short Damage { get; }
        public float KnockBack { get; }
        public short OriginalDamage { get; }
        public float AI2 { get; }
        public bool HasAI0 => (Flags & ProjectileNewFlags.HasAI0) != 0;
        public bool HasAI1 => (Flags & ProjectileNewFlags.HasAI1) != 0;
        public bool HasBannerIdToRespondTo => (Flags & ProjectileNewFlags.HasBannerIdToRespondTo) != 0;
        public bool HasDamage => (Flags & ProjectileNewFlags.HasDamage) != 0;
        public bool HasKnockBack => (Flags & ProjectileNewFlags.HasKnockBack) != 0;
        public bool HasOriginalDamage => (Flags & ProjectileNewFlags.HasOriginalDamage) != 0;
        public bool HasAI2 => (ExtraFlags & ProjectileNewExtraFlags.HasAI2) != 0;
    }
}
