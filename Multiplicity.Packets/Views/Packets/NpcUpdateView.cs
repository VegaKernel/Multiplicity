using System;
using System.IO;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NpcUpdateView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public NpcUpdateView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.NpcUpdate, payload))
        {
        }

        public static NpcUpdateView FromPayload(ReadOnlySpan<byte> payload) => new NpcUpdateView(payload);

        public NpcUpdateView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.NpcUpdate);

            PacketReader reader = packet.CreatePayloadReader();
            byte npcSlot = reader.ReadByte();
            byte generation = reader.ReadByte();
            float positionX = reader.ReadSingle();
            float positionY = reader.ReadSingle();
            float velocityX = reader.ReadSingle();
            float velocityY = reader.ReadSingle();
            ushort target = reader.ReadUInt16();
            NpcUpdateFlags flags = (NpcUpdateFlags)reader.ReadByte();
            NpcUpdateExtraFlags extraFlags = (NpcUpdateExtraFlags)reader.ReadByte();
            float ai0 = 0f;
            float ai1 = 0f;
            float ai2 = 0f;
            float ai3 = 0f;

            if ((flags & NpcUpdateFlags.HasAI0) != 0) ai0 = reader.ReadSingle();
            if ((flags & NpcUpdateFlags.HasAI1) != 0) ai1 = reader.ReadSingle();
            if ((flags & NpcUpdateFlags.HasAI2) != 0) ai2 = reader.ReadSingle();
            if ((flags & NpcUpdateFlags.HasAI3) != 0) ai3 = reader.ReadSingle();

            short npcNetId = reader.ReadInt16();
            byte statsScaledForPlayerCount = 1;
            float difficulty = 1f;
            byte lifeByteCount = 0;
            int life = 0;
            byte releaseOwner = byte.MaxValue;
            bool hasReleaseOwner = false;

            if ((extraFlags & NpcUpdateExtraFlags.HasPlayerCountScaling) != 0)
                statsScaledForPlayerCount = reader.ReadByte();
            if ((extraFlags & NpcUpdateExtraFlags.HasDifficultyOverride) != 0)
                difficulty = reader.ReadSingle();

            if ((flags & NpcUpdateFlags.LifeIsFull) == 0)
            {
                lifeByteCount = reader.ReadByte();
                life = lifeByteCount switch
                {
                    2 => reader.ReadInt16(),
                    4 => reader.ReadInt32(),
                    _ => reader.ReadSByte()
                };
            }

            if (reader.RemainingCount == 1)
            {
                hasReleaseOwner = true;
                releaseOwner = reader.ReadByte();
            }
            else if (reader.RemainingCount != 0)
            {
                throw new InvalidDataException($"NpcUpdate payload has {reader.RemainingCount} unexpected trailing bytes.");
            }

            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            NpcSlot = npcSlot;
            Generation = generation;
            PositionX = positionX;
            PositionY = positionY;
            VelocityX = velocityX;
            VelocityY = velocityY;
            Target = target;
            Flags = flags;
            ExtraFlags = extraFlags;
            AI0 = ai0;
            AI1 = ai1;
            AI2 = ai2;
            AI3 = ai3;
            NpcNetId = npcNetId;
            NpcType = NpcUpdate.NpcTypeFromNetId(npcNetId);
            StatsScaledForPlayerCount = statsScaledForPlayerCount;
            Difficulty = difficulty;
            LifeByteCount = lifeByteCount;
            Life = life;
            ReleaseOwner = releaseOwner;
            HasReleaseOwner = hasReleaseOwner;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public byte NpcSlot { get; }
        public byte Generation { get; }
        public float PositionX { get; }
        public float PositionY { get; }
        public float VelocityX { get; }
        public float VelocityY { get; }
        public ushort Target { get; }
        public NpcUpdateFlags Flags { get; }
        public NpcUpdateExtraFlags ExtraFlags { get; }
        public float AI0 { get; }
        public float AI1 { get; }
        public float AI2 { get; }
        public float AI3 { get; }
        public short NpcNetId { get; }
        public int NpcType { get; }
        public byte StatsScaledForPlayerCount { get; }
        public float Difficulty { get; }
        public byte LifeByteCount { get; }
        public int Life { get; }
        public byte ReleaseOwner { get; }
        public bool HasReleaseOwner { get; }
        public bool IsCatchable => HasReleaseOwner;
        public bool HasAI0 => (Flags & NpcUpdateFlags.HasAI0) != 0;
        public bool HasAI1 => (Flags & NpcUpdateFlags.HasAI1) != 0;
        public bool HasAI2 => (Flags & NpcUpdateFlags.HasAI2) != 0;
        public bool HasAI3 => (Flags & NpcUpdateFlags.HasAI3) != 0;
        public bool HasPlayerCountScaling => (ExtraFlags & NpcUpdateExtraFlags.HasPlayerCountScaling) != 0;
        public bool HasDifficultyOverride => (ExtraFlags & NpcUpdateExtraFlags.HasDifficultyOverride) != 0;
        public bool HasLifePayload => (Flags & NpcUpdateFlags.LifeIsFull) == 0;
    }
}
