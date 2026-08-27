using System;
using System.Collections.Generic;
using System.IO;

namespace Multiplicity.Packets
{
    [Flags]
    public enum NpcUpdateFlags : byte
    {
        None = 0,
        DirectionXPositive = 1 << 0,
        DirectionYPositive = 1 << 1,
        HasAI0 = 1 << 2,
        HasAI1 = 1 << 3,
        HasAI2 = 1 << 4,
        HasAI3 = 1 << 5,
        SpriteDirectionPositive = 1 << 6,
        LifeIsFull = 1 << 7
    }

    [Flags]
    public enum NpcUpdateExtraFlags : byte
    {
        None = 0,
        HasPlayerCountScaling = 1 << 0,
        SpawnedFromStatue = 1 << 1,
        HasDifficultyOverride = 1 << 2,
        SpawnNeedsSyncing = 1 << 3,
        HasShimmerTransparency = 1 << 4
    }

    public class NpcUpdate : TerrariaPacket
    {
        private const short FixedPayloadLength = 24;
        private const short SinglePayloadLength = 4;
        private const short BytePayloadLength = 1;
        private const byte LifeSizeSByte = 1;
        private const byte LifeSizeInt16 = 2;
        private const byte LifeSizeInt32 = 4;

        public static readonly int[] NetIDMap =
        {
            81,81,1,1,1,1,1,1,1,1,6,6,31,31,77,42,42,176,176,176,176,173,173,183,183,3,
            3,132,132,186,186,187,187,188,188,189,189,190,191,192,193,194,2,200,200,21,
            21,201,201,202,202,203,203,223,223,231,231,232,232,233,233,234,234,235,235
        };

        // Known vanilla catchable NPC ids retained only as a serialization convenience.
        // Deserialization does not depend on this table: ReleaseOwner is the final optional byte
        // in packet 23 and is detected from the actual payload length. This keeps protocol parsing
        // correct when Terraria adds another releasable critter without requiring a library update.
        public static readonly HashSet<short> NpcCatchable = new HashSet<short>
        {
            46,55,74,148,149,297,298,299,300,355,356,357,358,359,360,361,362,363,364,365,366,367,
            374,377,538,539,583,584,585,592,593,595,596,597,598,599,600,601,602,603,604,605,606,607,
            608,609,610,611,612,613,614,616,617,626,627,639,640,641,642,643,644,645,646,647,648,649,
            650,651,652,653,654,655,661,669,671,672,673,674,675,677,688,484,485,486,487
        };

        public byte NpcSlot { get; set; }
        public byte Generation { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public ushort Target { get; set; }
        public NpcUpdateFlags Flags { get; set; }
        public NpcUpdateExtraFlags ExtraFlags { get; set; }
        public float[] AI { get; set; }
        public short NpcNetId { get; set; }
        public byte StatsScaledForPlayerCount { get; set; } = 1;
        public float Difficulty { get; set; } = 1f;
        public byte LifeBytes { get; set; } = LifeSizeSByte;
        public int Life { get; set; }
        public byte ReleaseOwner { get; set; } = byte.MaxValue;

        /// <summary>
        /// Indicates that the optional ReleaseOwner tail byte is present on the wire.
        /// Incoming packets set this from the payload itself. When constructing a packet for a
        /// catchable NPC not present in <see cref="NpcCatchable"/>, set this to true explicitly.
        /// </summary>
        public bool HasReleaseOwner { get; set; }

        public int NpcType { get; set; }

        public NpcUpdate()
            : base((byte)PacketTypes.NpcUpdate)
        {
            AI = new float[4];
        }

        public NpcUpdate(BinaryReader br)
            : base(br)
        {
            AI = new float[4];

            NpcSlot = br.ReadByte();
            Generation = br.ReadByte();
            PositionX = br.ReadSingle();
            PositionY = br.ReadSingle();
            VelocityX = br.ReadSingle();
            VelocityY = br.ReadSingle();
            Target = br.ReadUInt16();
            Flags = (NpcUpdateFlags)br.ReadByte();
            ExtraFlags = (NpcUpdateExtraFlags)br.ReadByte();

            if (HasFlag(NpcUpdateFlags.HasAI0))
                AI[0] = br.ReadSingle();
            if (HasFlag(NpcUpdateFlags.HasAI1))
                AI[1] = br.ReadSingle();
            if (HasFlag(NpcUpdateFlags.HasAI2))
                AI[2] = br.ReadSingle();
            if (HasFlag(NpcUpdateFlags.HasAI3))
                AI[3] = br.ReadSingle();

            NpcNetId = br.ReadInt16();

            if (HasExtraFlag(NpcUpdateExtraFlags.HasPlayerCountScaling))
                StatsScaledForPlayerCount = br.ReadByte();
            if (HasExtraFlag(NpcUpdateExtraFlags.HasDifficultyOverride))
                Difficulty = br.ReadSingle();

            if (!HasFlag(NpcUpdateFlags.LifeIsFull))
            {
                LifeBytes = br.ReadByte();
                Life = LifeBytes switch
                {
                    LifeSizeInt16 => br.ReadInt16(),
                    LifeSizeInt32 => br.ReadInt32(),
                    _ => br.ReadSByte()
                };
            }

            NpcType = NpcTypeFromNetId(NpcNetId);

            long remaining = br.BaseStream.Length - br.BaseStream.Position;
            if (remaining == 1)
            {
                HasReleaseOwner = true;
                ReleaseOwner = br.ReadByte();
            }
            else if (remaining != 0)
            {
                throw new InvalidDataException($"NpcUpdate payload has {remaining} unexpected trailing bytes.");
            }
        }

        public static int NpcTypeFromNetId(int id)
        {
            return id < 0 ? NetIDMap[-id - 1] : id;
        }

        public override short GetLength()
        {
            NpcUpdateFlags flags = BuildFlags();
            NpcUpdateExtraFlags extraFlags = BuildExtraFlags();
            short length = FixedPayloadLength;

            if ((flags & NpcUpdateFlags.HasAI0) != 0)
                length += SinglePayloadLength;
            if ((flags & NpcUpdateFlags.HasAI1) != 0)
                length += SinglePayloadLength;
            if ((flags & NpcUpdateFlags.HasAI2) != 0)
                length += SinglePayloadLength;
            if ((flags & NpcUpdateFlags.HasAI3) != 0)
                length += SinglePayloadLength;
            if ((extraFlags & NpcUpdateExtraFlags.HasPlayerCountScaling) != 0)
                length += BytePayloadLength;
            if ((extraFlags & NpcUpdateExtraFlags.HasDifficultyOverride) != 0)
                length += SinglePayloadLength;
            if ((flags & NpcUpdateFlags.LifeIsFull) == 0)
                length += (short)(BytePayloadLength + GetLifePayloadLength());
            if (ShouldWriteReleaseOwner())
                length += BytePayloadLength;

            return length;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            NpcUpdateFlags flags = BuildFlags();
            NpcUpdateExtraFlags extraFlags = BuildExtraFlags();
            byte lifeSize = BuildLifeSize();

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(NpcSlot);
                bw.Write(Generation);
                bw.Write(PositionX);
                bw.Write(PositionY);
                bw.Write(VelocityX);
                bw.Write(VelocityY);
                bw.Write(Target);
                bw.Write((byte)flags);
                bw.Write((byte)extraFlags);

                if ((flags & NpcUpdateFlags.HasAI0) != 0)
                    bw.Write(AI[0]);
                if ((flags & NpcUpdateFlags.HasAI1) != 0)
                    bw.Write(AI[1]);
                if ((flags & NpcUpdateFlags.HasAI2) != 0)
                    bw.Write(AI[2]);
                if ((flags & NpcUpdateFlags.HasAI3) != 0)
                    bw.Write(AI[3]);

                bw.Write(NpcNetId);

                if ((extraFlags & NpcUpdateExtraFlags.HasPlayerCountScaling) != 0)
                    bw.Write(StatsScaledForPlayerCount);
                if ((extraFlags & NpcUpdateExtraFlags.HasDifficultyOverride) != 0)
                    bw.Write(Difficulty);

                if ((flags & NpcUpdateFlags.LifeIsFull) == 0)
                {
                    bw.Write(lifeSize);
                    switch (lifeSize)
                    {
                        case LifeSizeInt16:
                            bw.Write((short)Life);
                            break;
                        case LifeSizeInt32:
                            bw.Write(Life);
                            break;
                        default:
                            bw.Write((sbyte)Life);
                            break;
                    }
                }

                if (ShouldWriteReleaseOwner())
                    bw.Write(ReleaseOwner);
            }
        }

        public override string ToString()
        {
            return $"[NpcUpdate: NpcSlot = {NpcSlot}, Generation = {Generation}, PositionX = {PositionX}, PositionY = {PositionY}, VelocityX = {VelocityX}, VelocityY = {VelocityY}, Target = {Target}, Flags = {Flags}, ExtraFlags = {ExtraFlags}, HasReleaseOwner = {HasReleaseOwner}]";
        }

        private bool HasFlag(NpcUpdateFlags flag)
        {
            return (Flags & flag) != 0;
        }

        private bool HasExtraFlag(NpcUpdateExtraFlags flag)
        {
            return (ExtraFlags & flag) != 0;
        }

        private NpcUpdateFlags BuildFlags()
        {
            NpcUpdateFlags flags = Flags;
            flags = SetFlag(flags, NpcUpdateFlags.HasAI0, HasExplicitFlag(Flags, NpcUpdateFlags.HasAI0) || AI[0] != 0f);
            flags = SetFlag(flags, NpcUpdateFlags.HasAI1, HasExplicitFlag(Flags, NpcUpdateFlags.HasAI1) || AI[1] != 0f);
            flags = SetFlag(flags, NpcUpdateFlags.HasAI2, HasExplicitFlag(Flags, NpcUpdateFlags.HasAI2) || AI[2] != 0f);
            flags = SetFlag(flags, NpcUpdateFlags.HasAI3, HasExplicitFlag(Flags, NpcUpdateFlags.HasAI3) || AI[3] != 0f);
            flags = SetFlag(flags, NpcUpdateFlags.LifeIsFull, HasExplicitFlag(Flags, NpcUpdateFlags.LifeIsFull) || LifeBytes == 0);
            return flags;
        }

        private NpcUpdateExtraFlags BuildExtraFlags()
        {
            NpcUpdateExtraFlags extraFlags = ExtraFlags;
            extraFlags = SetFlag(extraFlags, NpcUpdateExtraFlags.HasPlayerCountScaling, HasExplicitFlag(ExtraFlags, NpcUpdateExtraFlags.HasPlayerCountScaling) || StatsScaledForPlayerCount > 1);
            extraFlags = SetFlag(extraFlags, NpcUpdateExtraFlags.HasDifficultyOverride, HasExplicitFlag(ExtraFlags, NpcUpdateExtraFlags.HasDifficultyOverride) || Difficulty != 1f);
            return extraFlags;
        }

        private bool ShouldWriteReleaseOwner()
        {
            return HasReleaseOwner || IsKnownCatchableType(NpcTypeFromNetId(NpcNetId));
        }

        private byte BuildLifeSize()
        {
            if (LifeBytes == LifeSizeInt32)
                return LifeSizeInt32;

            if (LifeBytes == LifeSizeInt16 && Life >= short.MinValue && Life <= short.MaxValue)
                return LifeSizeInt16;

            if (LifeBytes == LifeSizeSByte && Life >= sbyte.MinValue && Life <= sbyte.MaxValue)
                return LifeSizeSByte;

            if (Life > short.MaxValue || Life < short.MinValue)
                return LifeSizeInt32;

            if (Life > sbyte.MaxValue || Life < sbyte.MinValue)
                return LifeSizeInt16;

            return LifeSizeSByte;
        }

        private int GetLifePayloadLength()
        {
            return BuildLifeSize() switch
            {
                LifeSizeInt16 => sizeof(short),
                LifeSizeInt32 => sizeof(int),
                _ => sizeof(sbyte)
            };
        }

        private static bool IsKnownCatchableType(int npcType)
        {
            return npcType >= 0 && NpcCatchable.Contains((short)npcType);
        }

        private static bool HasExplicitFlag(NpcUpdateFlags current, NpcUpdateFlags flag)
        {
            return (current & flag) != 0;
        }

        private static bool HasExplicitFlag(NpcUpdateExtraFlags current, NpcUpdateExtraFlags flag)
        {
            return (current & flag) != 0;
        }

        private static NpcUpdateFlags SetFlag(NpcUpdateFlags current, NpcUpdateFlags flag, bool enabled)
        {
            return enabled ? current | flag : current & ~flag;
        }

        private static NpcUpdateExtraFlags SetFlag(NpcUpdateExtraFlags current, NpcUpdateExtraFlags flag, bool enabled)
        {
            return enabled ? current | flag : current & ~flag;
        }
    }
}
