using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The ProjectileUpdate (0x1B) packet for protocol 317/318.
    /// </summary>
    public class ProjectileUpdate : TerrariaPacket
    {
        private const short FixedPayloadLength = 22;
        private const short ExtraFlagsPayloadLength = 1;
        private const short SinglePayloadLength = 4;
        private const short ShortPayloadLength = 2;

        public short ProjectileID { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public byte Owner { get; set; }
        public short Type { get; set; }
        public ProjectileUpdateFlags Flags { get; set; }
        public ProjectileUpdateExtraFlags ExtraFlags { get; set; }
        public float AI0 { get; set; }
        public float AI1 { get; set; }
        public ushort BannerIdToRespondTo { get; set; }
        public short Damage { get; set; }
        public float KnockBack { get; set; }
        public short OriginalDamage { get; set; }
        public short ProjUUID { get; set; } = -1;
        public float AI2 { get; set; }

        public ProjectileUpdate()
            : base((byte)PacketTypes.ProjectileUpdate)
        {
        }

        public ProjectileUpdate(BinaryReader br)
            : base(br)
        {
            ProjectileID = br.ReadInt16();
            PositionX = br.ReadSingle();
            PositionY = br.ReadSingle();
            VelocityX = br.ReadSingle();
            VelocityY = br.ReadSingle();
            Owner = br.ReadByte();
            Type = br.ReadInt16();
            Flags = (ProjectileUpdateFlags)br.ReadByte();
            ExtraFlags = (Flags & ProjectileUpdateFlags.HasExtraFlags) != 0
                ? (ProjectileUpdateExtraFlags)br.ReadByte()
                : ProjectileUpdateExtraFlags.None;

            if (HasFlag(ProjectileUpdateFlags.HasAI0))
                AI0 = br.ReadSingle();
            if (HasFlag(ProjectileUpdateFlags.HasAI1))
                AI1 = br.ReadSingle();
            if (HasFlag(ProjectileUpdateFlags.HasBannerIdToRespondTo))
                BannerIdToRespondTo = br.ReadUInt16();
            if (HasFlag(ProjectileUpdateFlags.HasDamage))
                Damage = br.ReadInt16();
            if (HasFlag(ProjectileUpdateFlags.HasKnockBack))
                KnockBack = br.ReadSingle();
            if (HasFlag(ProjectileUpdateFlags.HasOriginalDamage))
                OriginalDamage = br.ReadInt16();
            if (HasFlag(ProjectileUpdateFlags.HasProjectileUuid))
                ProjUUID = br.ReadInt16();
            if ((ExtraFlags & ProjectileUpdateExtraFlags.HasAI2) != 0)
                AI2 = br.ReadSingle();
        }

        public override short GetLength()
        {
            ProjectileUpdateFlags flags = BuildFlags();
            ProjectileUpdateExtraFlags extraFlags = BuildExtraFlags();
            short length = FixedPayloadLength;

            if ((flags & ProjectileUpdateFlags.HasExtraFlags) != 0)
                length += ExtraFlagsPayloadLength;
            if ((flags & ProjectileUpdateFlags.HasAI0) != 0)
                length += SinglePayloadLength;
            if ((flags & ProjectileUpdateFlags.HasAI1) != 0)
                length += SinglePayloadLength;
            if ((flags & ProjectileUpdateFlags.HasBannerIdToRespondTo) != 0)
                length += ShortPayloadLength;
            if ((flags & ProjectileUpdateFlags.HasDamage) != 0)
                length += ShortPayloadLength;
            if ((flags & ProjectileUpdateFlags.HasKnockBack) != 0)
                length += SinglePayloadLength;
            if ((flags & ProjectileUpdateFlags.HasOriginalDamage) != 0)
                length += ShortPayloadLength;
            if ((flags & ProjectileUpdateFlags.HasProjectileUuid) != 0)
                length += ShortPayloadLength;
            if ((extraFlags & ProjectileUpdateExtraFlags.HasAI2) != 0)
                length += SinglePayloadLength;

            return length;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            ProjectileUpdateFlags flags = BuildFlags();
            ProjectileUpdateExtraFlags extraFlags = BuildExtraFlags();

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(ProjectileID);
                bw.Write(PositionX);
                bw.Write(PositionY);
                bw.Write(VelocityX);
                bw.Write(VelocityY);
                bw.Write(Owner);
                bw.Write(Type);
                bw.Write((byte)flags);
                if ((flags & ProjectileUpdateFlags.HasExtraFlags) != 0)
                    bw.Write((byte)extraFlags);
                if ((flags & ProjectileUpdateFlags.HasAI0) != 0)
                    bw.Write(AI0);
                if ((flags & ProjectileUpdateFlags.HasAI1) != 0)
                    bw.Write(AI1);
                if ((flags & ProjectileUpdateFlags.HasBannerIdToRespondTo) != 0)
                    bw.Write(BannerIdToRespondTo);
                if ((flags & ProjectileUpdateFlags.HasDamage) != 0)
                    bw.Write(Damage);
                if ((flags & ProjectileUpdateFlags.HasKnockBack) != 0)
                    bw.Write(KnockBack);
                if ((flags & ProjectileUpdateFlags.HasOriginalDamage) != 0)
                    bw.Write(OriginalDamage);
                if ((flags & ProjectileUpdateFlags.HasProjectileUuid) != 0)
                    bw.Write(ProjUUID);
                if ((extraFlags & ProjectileUpdateExtraFlags.HasAI2) != 0)
                    bw.Write(AI2);
            }
        }

        public override string ToString()
        {
            return $"[ProjectileUpdate: ProjectileID = {ProjectileID} PositionX = {PositionX} PositionY = {PositionY} VelocityX = {VelocityX} VelocityY = {VelocityY} Owner = {Owner} Type = {Type} Flags = {Flags} ExtraFlags = {ExtraFlags}]";
        }

        private bool HasFlag(ProjectileUpdateFlags flag)
        {
            return (Flags & flag) != 0;
        }

        private ProjectileUpdateFlags BuildFlags()
        {
            ProjectileUpdateFlags flags = Flags;
            flags = SetFlag(flags, ProjectileUpdateFlags.HasAI0, AI0 != 0f);
            flags = SetFlag(flags, ProjectileUpdateFlags.HasAI1, AI1 != 0f);
            flags = SetFlag(flags, ProjectileUpdateFlags.HasBannerIdToRespondTo, BannerIdToRespondTo != 0);
            flags = SetFlag(flags, ProjectileUpdateFlags.HasDamage, Damage != 0);
            flags = SetFlag(flags, ProjectileUpdateFlags.HasKnockBack, KnockBack != 0f);
            flags = SetFlag(flags, ProjectileUpdateFlags.HasOriginalDamage, OriginalDamage != 0);
            flags = SetFlag(flags, ProjectileUpdateFlags.HasProjectileUuid, ProjUUID >= 0);
            flags = SetFlag(flags, ProjectileUpdateFlags.HasExtraFlags, BuildExtraFlags() != ProjectileUpdateExtraFlags.None);
            return flags;
        }

        private ProjectileUpdateExtraFlags BuildExtraFlags()
        {
            ProjectileUpdateExtraFlags extraFlags = ExtraFlags;
            extraFlags = SetFlag(extraFlags, ProjectileUpdateExtraFlags.HasAI2, AI2 != 0f);
            return extraFlags;
        }

        private static ProjectileUpdateFlags SetFlag(ProjectileUpdateFlags current, ProjectileUpdateFlags flag, bool enabled)
        {
            return enabled ? current | flag : current & ~flag;
        }

        private static ProjectileUpdateExtraFlags SetFlag(ProjectileUpdateExtraFlags current, ProjectileUpdateExtraFlags flag, bool enabled)
        {
            return enabled ? current | flag : current & ~flag;
        }
    }
}
