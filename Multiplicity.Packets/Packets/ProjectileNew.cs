using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The ProjectileNew (0x1B) packet for protocol 325.
    /// </summary>
    public class ProjectileNew : TerrariaPacket
    {
        private const short FixedPayloadLength = 23;
        private const short ExtraFlagsPayloadLength = 1;
        private const short SinglePayloadLength = 4;
        private const short ShortPayloadLength = 2;

        public ProjectileKey Key { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public short Type { get; set; }
        public ProjectileNewFlags Flags { get; set; }
        public ProjectileNewExtraFlags ExtraFlags { get; set; }
        public float AI0 { get; set; }
        public float AI1 { get; set; }
        public ushort BannerIdToRespondTo { get; set; }
        public short Damage { get; set; }
        public float KnockBack { get; set; }
        public short OriginalDamage { get; set; }
        public float AI2 { get; set; }

        public ProjectileNew()
            : base((byte)PacketTypes.ProjectileNew)
        {
        }

        public ProjectileNew(BinaryReader br)
            : base(br)
        {
            Key = ProjectileKey.FromPacked(br.ReadInt32());
            PositionX = br.ReadSingle();
            PositionY = br.ReadSingle();
            VelocityX = br.ReadSingle();
            VelocityY = br.ReadSingle();
            Type = br.ReadInt16();
            Flags = (ProjectileNewFlags)br.ReadByte();
            ExtraFlags = (Flags & ProjectileNewFlags.HasExtraFlags) != 0
                ? (ProjectileNewExtraFlags)br.ReadByte()
                : ProjectileNewExtraFlags.None;

            if (HasFlag(ProjectileNewFlags.HasAI0)) AI0 = br.ReadSingle();
            if (HasFlag(ProjectileNewFlags.HasAI1)) AI1 = br.ReadSingle();
            if (HasFlag(ProjectileNewFlags.HasBannerIdToRespondTo)) BannerIdToRespondTo = br.ReadUInt16();
            if (HasFlag(ProjectileNewFlags.HasDamage)) Damage = br.ReadInt16();
            if (HasFlag(ProjectileNewFlags.HasKnockBack)) KnockBack = br.ReadSingle();
            if (HasFlag(ProjectileNewFlags.HasOriginalDamage)) OriginalDamage = br.ReadInt16();
            if ((ExtraFlags & ProjectileNewExtraFlags.HasAI2) != 0) AI2 = br.ReadSingle();
        }

        public override short GetLength()
        {
            ProjectileNewFlags flags = BuildFlags();
            ProjectileNewExtraFlags extraFlags = BuildExtraFlags();
            short length = FixedPayloadLength;

            if ((flags & ProjectileNewFlags.HasExtraFlags) != 0) length += ExtraFlagsPayloadLength;
            if ((flags & ProjectileNewFlags.HasAI0) != 0) length += SinglePayloadLength;
            if ((flags & ProjectileNewFlags.HasAI1) != 0) length += SinglePayloadLength;
            if ((flags & ProjectileNewFlags.HasBannerIdToRespondTo) != 0) length += ShortPayloadLength;
            if ((flags & ProjectileNewFlags.HasDamage) != 0) length += ShortPayloadLength;
            if ((flags & ProjectileNewFlags.HasKnockBack) != 0) length += SinglePayloadLength;
            if ((flags & ProjectileNewFlags.HasOriginalDamage) != 0) length += ShortPayloadLength;
            if ((extraFlags & ProjectileNewExtraFlags.HasAI2) != 0) length += SinglePayloadLength;

            return length;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
                base.ToStream(stream, includeHeader);

            ProjectileNewFlags flags = BuildFlags();
            ProjectileNewExtraFlags extraFlags = BuildExtraFlags();

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(Key.ToInt32());
                bw.Write(PositionX);
                bw.Write(PositionY);
                bw.Write(VelocityX);
                bw.Write(VelocityY);
                bw.Write(Type);
                bw.Write((byte)flags);
                if ((flags & ProjectileNewFlags.HasExtraFlags) != 0) bw.Write((byte)extraFlags);
                if ((flags & ProjectileNewFlags.HasAI0) != 0) bw.Write(AI0);
                if ((flags & ProjectileNewFlags.HasAI1) != 0) bw.Write(AI1);
                if ((flags & ProjectileNewFlags.HasBannerIdToRespondTo) != 0) bw.Write(BannerIdToRespondTo);
                if ((flags & ProjectileNewFlags.HasDamage) != 0) bw.Write(Damage);
                if ((flags & ProjectileNewFlags.HasKnockBack) != 0) bw.Write(KnockBack);
                if ((flags & ProjectileNewFlags.HasOriginalDamage) != 0) bw.Write(OriginalDamage);
                if ((extraFlags & ProjectileNewExtraFlags.HasAI2) != 0) bw.Write(AI2);
            }
        }

        public override string ToString()
        {
            return $"[ProjectileNew: Key = {Key} PositionX = {PositionX} PositionY = {PositionY} VelocityX = {VelocityX} VelocityY = {VelocityY} Type = {Type} Flags = {Flags} ExtraFlags = {ExtraFlags}]";
        }

        private bool HasFlag(ProjectileNewFlags flag) => (Flags & flag) != 0;

        private ProjectileNewFlags BuildFlags()
        {
            ProjectileNewFlags flags = Flags;
            flags = SetFlag(flags, ProjectileNewFlags.HasAI0, AI0 != 0f);
            flags = SetFlag(flags, ProjectileNewFlags.HasAI1, AI1 != 0f);
            flags = SetFlag(flags, ProjectileNewFlags.HasBannerIdToRespondTo, BannerIdToRespondTo != 0);
            flags = SetFlag(flags, ProjectileNewFlags.HasDamage, Damage != 0);
            flags = SetFlag(flags, ProjectileNewFlags.HasKnockBack, KnockBack != 0f);
            flags = SetFlag(flags, ProjectileNewFlags.HasOriginalDamage, OriginalDamage != 0);
            flags = SetFlag(flags, ProjectileNewFlags.HasExtraFlags, BuildExtraFlags() != ProjectileNewExtraFlags.None);
            return flags;
        }

        private ProjectileNewExtraFlags BuildExtraFlags()
        {
            ProjectileNewExtraFlags extraFlags = ExtraFlags;
            extraFlags = SetFlag(extraFlags, ProjectileNewExtraFlags.HasAI2, AI2 != 0f);
            return extraFlags;
        }

        private static ProjectileNewFlags SetFlag(ProjectileNewFlags current, ProjectileNewFlags flag, bool enabled) =>
            enabled ? current | flag : current & ~flag;

        private static ProjectileNewExtraFlags SetFlag(ProjectileNewExtraFlags current, ProjectileNewExtraFlags flag, bool enabled) =>
            enabled ? current | flag : current & ~flag;
    }
}
