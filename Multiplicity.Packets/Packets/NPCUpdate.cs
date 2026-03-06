using System;
using System.Collections.Generic;
using System.IO;

namespace Multiplicity.Packets
{
    [Flags]
    public enum NPCUpdateFlags : byte
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
    public enum NPCUpdateExtraFlags : byte
    {
        None = 0,
        HasPlayerCountScaling = 1 << 0,
        SpawnedFromStatue = 1 << 1,
        HasDifficultyOverride = 1 << 2,
        SpawnNeedsSyncing = 1 << 3,
        HasShimmerTransparency = 1 << 4
    }

    public class NPCUpdate : TerrariaPacket
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

        public short NPCID { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public ushort Target { get; set; }
        public NPCUpdateFlags Flags { get; set; }
        public NPCUpdateExtraFlags ExtraFlags { get; set; }
        public float[] AI { get; set; }
        public short NPCNetID { get; set; }
        public byte StatsScaledForPlayerCount { get; set; } = 1;
        public float Difficulty { get; set; } = 1f;
        public byte LifeBytes { get; set; } = LifeSizeSByte;
        public int Life { get; set; }
        public byte ReleaseOwner { get; set; }
        public int NPCType { get; set; }

        public NPCUpdate()
            : base((byte)PacketTypes.NPCUpdate)
        {
            AI = new float[4];
        }

        public NPCUpdate(BinaryReader br)
            : base(br)
        {
            AI = new float[4];

            NPCID = br.ReadInt16();
            PositionX = br.ReadSingle();
            PositionY = br.ReadSingle();
            VelocityX = br.ReadSingle();
            VelocityY = br.ReadSingle();
            Target = br.ReadUInt16();
            Flags = (NPCUpdateFlags)br.ReadByte();
            ExtraFlags = (NPCUpdateExtraFlags)br.ReadByte();

            if (HasFlag(NPCUpdateFlags.HasAI0))
                AI[0] = br.ReadSingle();
            if (HasFlag(NPCUpdateFlags.HasAI1))
                AI[1] = br.ReadSingle();
            if (HasFlag(NPCUpdateFlags.HasAI2))
                AI[2] = br.ReadSingle();
            if (HasFlag(NPCUpdateFlags.HasAI3))
                AI[3] = br.ReadSingle();

            NPCNetID = br.ReadInt16();

            if (HasExtraFlag(NPCUpdateExtraFlags.HasPlayerCountScaling))
                StatsScaledForPlayerCount = br.ReadByte();
            if (HasExtraFlag(NPCUpdateExtraFlags.HasDifficultyOverride))
                Difficulty = br.ReadSingle();

            if (!HasFlag(NPCUpdateFlags.LifeIsFull))
            {
                LifeBytes = br.ReadByte();
                Life = LifeBytes switch
                {
                    LifeSizeInt16 => br.ReadInt16(),
                    LifeSizeInt32 => br.ReadInt32(),
                    _ => br.ReadSByte()
                };
            }

            NPCType = NPCTypeFromNetID(NPCNetID);
            if (IsCatchableType(NPCType))
                ReleaseOwner = br.ReadByte();
        }

        public static int NPCTypeFromNetID(int id)
        {
            return id < 0 ? NetIDMap[-id - 1] : id;
        }

        public override short GetLength()
        {
            NPCUpdateFlags flags = BuildFlags();
            NPCUpdateExtraFlags extraFlags = BuildExtraFlags();
            short length = FixedPayloadLength;

            if ((flags & NPCUpdateFlags.HasAI0) != 0)
                length += SinglePayloadLength;
            if ((flags & NPCUpdateFlags.HasAI1) != 0)
                length += SinglePayloadLength;
            if ((flags & NPCUpdateFlags.HasAI2) != 0)
                length += SinglePayloadLength;
            if ((flags & NPCUpdateFlags.HasAI3) != 0)
                length += SinglePayloadLength;
            if ((extraFlags & NPCUpdateExtraFlags.HasPlayerCountScaling) != 0)
                length += BytePayloadLength;
            if ((extraFlags & NPCUpdateExtraFlags.HasDifficultyOverride) != 0)
                length += SinglePayloadLength;
            if ((flags & NPCUpdateFlags.LifeIsFull) == 0)
                length += (short)(BytePayloadLength + GetLifePayloadLength());
            if (IsCatchableType(NPCTypeFromNetID(NPCNetID)))
                length += BytePayloadLength;

            return length;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            NPCUpdateFlags flags = BuildFlags();
            NPCUpdateExtraFlags extraFlags = BuildExtraFlags();
            byte lifeSize = BuildLifeSize();

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(NPCID);
                bw.Write(PositionX);
                bw.Write(PositionY);
                bw.Write(VelocityX);
                bw.Write(VelocityY);
                bw.Write(Target);
                bw.Write((byte)flags);
                bw.Write((byte)extraFlags);

                if ((flags & NPCUpdateFlags.HasAI0) != 0)
                    bw.Write(AI[0]);
                if ((flags & NPCUpdateFlags.HasAI1) != 0)
                    bw.Write(AI[1]);
                if ((flags & NPCUpdateFlags.HasAI2) != 0)
                    bw.Write(AI[2]);
                if ((flags & NPCUpdateFlags.HasAI3) != 0)
                    bw.Write(AI[3]);

                bw.Write(NPCNetID);

                if ((extraFlags & NPCUpdateExtraFlags.HasPlayerCountScaling) != 0)
                    bw.Write(StatsScaledForPlayerCount);
                if ((extraFlags & NPCUpdateExtraFlags.HasDifficultyOverride) != 0)
                    bw.Write(Difficulty);

                if ((flags & NPCUpdateFlags.LifeIsFull) == 0)
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

                int npcType = NPCTypeFromNetID(NPCNetID);
                if (IsCatchableType(npcType))
                    bw.Write(ReleaseOwner);
            }
        }

        public override string ToString()
        {
            return $"[NPCUpdate: NPCID = {NPCID}, PositionX = {PositionX}, PositionY = {PositionY}, VelocityX = {VelocityX}, VelocityY = {VelocityY}, Target = {Target}, Flags = {Flags}, ExtraFlags = {ExtraFlags}]";
        }

        private bool HasFlag(NPCUpdateFlags flag)
        {
            return (Flags & flag) != 0;
        }

        private bool HasExtraFlag(NPCUpdateExtraFlags flag)
        {
            return (ExtraFlags & flag) != 0;
        }

        private NPCUpdateFlags BuildFlags()
        {
            NPCUpdateFlags flags = Flags;
            flags = SetFlag(flags, NPCUpdateFlags.HasAI0, AI[0] != 0f);
            flags = SetFlag(flags, NPCUpdateFlags.HasAI1, AI[1] != 0f);
            flags = SetFlag(flags, NPCUpdateFlags.HasAI2, AI[2] != 0f);
            flags = SetFlag(flags, NPCUpdateFlags.HasAI3, AI[3] != 0f);
            flags = SetFlag(flags, NPCUpdateFlags.LifeIsFull, LifeBytes == 0);
            return flags;
        }

        private NPCUpdateExtraFlags BuildExtraFlags()
        {
            NPCUpdateExtraFlags extraFlags = ExtraFlags;
            extraFlags = SetFlag(extraFlags, NPCUpdateExtraFlags.HasPlayerCountScaling, StatsScaledForPlayerCount > 1);
            extraFlags = SetFlag(extraFlags, NPCUpdateExtraFlags.HasDifficultyOverride, Difficulty != 1f);
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

        private static NPCUpdateFlags SetFlag(NPCUpdateFlags current, NPCUpdateFlags flag, bool enabled)
        {
            return enabled ? current | flag : current & ~flag;
        }

        private static NPCUpdateExtraFlags SetFlag(NPCUpdateExtraFlags current, NPCUpdateExtraFlags flag, bool enabled)
        {
            return enabled ? current | flag : current & ~flag;
        }
    }
}
