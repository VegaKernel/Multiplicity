using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NpcUpdateView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public NpcUpdateView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.NPCUpdate);

            PacketReader reader = packet.CreatePayloadReader();
            short npcId = reader.ReadInt16();
            float positionX = reader.ReadSingle();
            float positionY = reader.ReadSingle();
            float velocityX = reader.ReadSingle();
            float velocityY = reader.ReadSingle();
            ushort target = reader.ReadUInt16();
            NPCUpdateFlags flags = (NPCUpdateFlags)reader.ReadByte();
            NPCUpdateExtraFlags extraFlags = (NPCUpdateExtraFlags)reader.ReadByte();
            float ai0 = 0f;
            float ai1 = 0f;
            float ai2 = 0f;
            float ai3 = 0f;

            if ((flags & NPCUpdateFlags.HasAI0) != 0)
            {
                ai0 = reader.ReadSingle();
            }

            if ((flags & NPCUpdateFlags.HasAI1) != 0)
            {
                ai1 = reader.ReadSingle();
            }

            if ((flags & NPCUpdateFlags.HasAI2) != 0)
            {
                ai2 = reader.ReadSingle();
            }

            if ((flags & NPCUpdateFlags.HasAI3) != 0)
            {
                ai3 = reader.ReadSingle();
            }

            short npcNetId = reader.ReadInt16();
            byte statsScaledForPlayerCount = 1;
            float difficulty = 1f;
            byte lifeByteCount = 0;
            int life = 0;
            byte releaseOwner = 0;

            if ((extraFlags & NPCUpdateExtraFlags.HasPlayerCountScaling) != 0)
            {
                statsScaledForPlayerCount = reader.ReadByte();
            }

            if ((extraFlags & NPCUpdateExtraFlags.HasDifficultyOverride) != 0)
            {
                difficulty = reader.ReadSingle();
            }

            if ((flags & NPCUpdateFlags.LifeIsFull) == 0)
            {
                lifeByteCount = reader.ReadByte();
                life = lifeByteCount switch
                {
                    2 => reader.ReadInt16(),
                    4 => reader.ReadInt32(),
                    _ => reader.ReadSByte()
                };
            }

            int npcType = NPCUpdate.NPCTypeFromNetID(npcNetId);
            bool isCatchable = npcType >= 0 && NPCUpdate.NpcCatchable.Contains((short)npcType);
            if (isCatchable)
            {
                releaseOwner = reader.ReadByte();
            }

            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            NpcId = npcId;
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
            NpcType = npcType;
            StatsScaledForPlayerCount = statsScaledForPlayerCount;
            Difficulty = difficulty;
            LifeByteCount = lifeByteCount;
            Life = life;
            ReleaseOwner = releaseOwner;
            IsCatchable = isCatchable;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public short NpcId { get; }

        public float PositionX { get; }

        public float PositionY { get; }

        public float VelocityX { get; }

        public float VelocityY { get; }

        public ushort Target { get; }

        public NPCUpdateFlags Flags { get; }

        public NPCUpdateExtraFlags ExtraFlags { get; }

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

        public bool IsCatchable { get; }

        public bool HasAI0 => (Flags & NPCUpdateFlags.HasAI0) != 0;

        public bool HasAI1 => (Flags & NPCUpdateFlags.HasAI1) != 0;

        public bool HasAI2 => (Flags & NPCUpdateFlags.HasAI2) != 0;

        public bool HasAI3 => (Flags & NPCUpdateFlags.HasAI3) != 0;

        public bool HasPlayerCountScaling => (ExtraFlags & NPCUpdateExtraFlags.HasPlayerCountScaling) != 0;

        public bool HasDifficultyOverride => (ExtraFlags & NPCUpdateExtraFlags.HasDifficultyOverride) != 0;

        public bool HasLifePayload => (Flags & NPCUpdateFlags.LifeIsFull) == 0;
    }
}
