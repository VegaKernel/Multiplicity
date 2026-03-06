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

        public static readonly HashSet<short> NpcCatchable = new HashSet<short>
        {
            46,55,74,148,149,297,298,299,300,355,356,357,358,359,360,361,362,363,364,
            365,366,367,374,377,539,538,484,485,486,487,442,443,444,445,446,447,448
        };

        public short NpcId { get; set; }
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
        public byte ReleaseOwner { get; set; }
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

            NpcId = br.ReadInt16();
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
            if (IsCatchableType(NpcType))
                ReleaseOwner = br.ReadByte();
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
            if (IsCatchableType(NpcTypeFromNetId(NpcNetId)))
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
                bw.Write(NpcId);
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

                int npcType = NpcTypeFromNetId(NpcNetId);
                if (IsCatchableType(npcType))
                    bw.Write(ReleaseOwner);
            }
        }

        public override string ToString()
        {
            return $"[NpcUpdate: NpcId = {NpcId}, PositionX = {PositionX}, PositionY = {PositionY}, VelocityX = {VelocityX}, VelocityY = {VelocityY}, Target = {Target}, Flags = {Flags}, ExtraFlags = {ExtraFlags}]";
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
            flags = SetFlag(flags, NpcUpdateFlags.HasAI0, AI[0] != 0f);
            flags = SetFlag(flags, NpcUpdateFlags.HasAI1, AI[1] != 0f);
            flags = SetFlag(flags, NpcUpdateFlags.HasAI2, AI[2] != 0f);
            flags = SetFlag(flags, NpcUpdateFlags.HasAI3, AI[3] != 0f);
            flags = SetFlag(flags, NpcUpdateFlags.LifeIsFull, LifeBytes == 0);
            return flags;
        }

        private NpcUpdateExtraFlags BuildExtraFlags()
        {
            NpcUpdateExtraFlags extraFlags = ExtraFlags;
            extraFlags = SetFlag(extraFlags, NpcUpdateExtraFlags.HasPlayerCountScaling, StatsScaledForPlayerCount > 1);
            extraFlags = SetFlag(extraFlags, NpcUpdateExtraFlags.HasDifficultyOverride, Difficulty != 1f);
            return extraFlags;
        }

        private byte BuildLifeSize()
        {
            if (LifeBytes == LifeSizeSByte || LifeBytes == LifeSizeInt16 || LifeBytes == LifeSizeInt32)
                return LifeBytes;

            if (Life > sbyte.MaxValue || Life < sbyte.MinValue)
            {
                return Life > short.MaxValue || Life < short.MinValue ? LifeSizeInt32 : LifeSizeInt16;
            }

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

        private static bool IsCatchableType(int npcType)
        {
            return npcType >= 0 && NpcCatchable.Contains((short)npcType);
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

