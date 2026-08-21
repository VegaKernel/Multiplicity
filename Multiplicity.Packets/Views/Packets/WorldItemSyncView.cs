using System;
using System.IO;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct WorldItemSyncView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public WorldItemSyncView(PacketTypes packetType, ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(packetType, payload))
        {
        }

        public static WorldItemSyncView FromPayload(PacketTypes packetType, ReadOnlySpan<byte> payload) =>
            new WorldItemSyncView(packetType, payload);

        public WorldItemSyncView(PacketView packet)
        {
            if (!IsSupported(packet.PacketType))
                throw new InvalidDataException($"Packet type {packet.PacketType} is not a supported world item sync packet.");

            PacketReader reader = packet.CreatePayloadReader();
            short itemIndex = reader.ReadInt16();
            float positionX = reader.ReadSingle();
            float positionY = reader.ReadSingle();
            float velocityX = reader.ReadSingle();
            float velocityY = reader.ReadSingle();
            short stack = reader.ReadInt16();
            byte prefix = reader.ReadByte();
            WorldItemSyncFlags flags = (WorldItemSyncFlags)reader.ReadByte();
            short itemNetId = reader.ReadInt16();
            bool shimmered = false;
            float shimmerTime = 0f;
            byte enemyGrabDelayTime = 0;

            if ((flags & WorldItemSyncFlags.HasShimmerData) != 0)
            {
                shimmered = reader.ReadBoolean();
                shimmerTime = reader.ReadSingle();
            }

            if ((flags & WorldItemSyncFlags.HasEnemyGrabDelay) != 0)
                enemyGrabDelayTime = reader.ReadByte();

            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            ItemIndex = itemIndex;
            PositionX = positionX;
            PositionY = positionY;
            VelocityX = velocityX;
            VelocityY = velocityY;
            Stack = stack;
            Prefix = prefix;
            Flags = flags;
            ItemNetId = itemNetId;
            Shimmered = shimmered;
            ShimmerTime = shimmerTime;
            EnemyGrabDelayTime = enemyGrabDelayTime;
        }

        public PacketHeader Header { get; }
        public PacketTypes PacketType => Header.PacketType;
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public short ItemIndex { get; }
        public float PositionX { get; }
        public float PositionY { get; }
        public float VelocityX { get; }
        public float VelocityY { get; }
        public short Stack { get; }
        public byte Prefix { get; }
        public WorldItemSyncFlags Flags { get; }
        public short ItemNetId { get; }
        public bool Shimmered { get; }
        public float ShimmerTime { get; }
        public byte EnemyGrabDelayTime { get; }
        public NewItemOwnership Ownership => (NewItemOwnership)((byte)Flags & (byte)WorldItemSyncFlags.OwnershipMask);
        public bool HasShimmerData => (Flags & WorldItemSyncFlags.HasShimmerData) != 0;
        public bool HasEnemyGrabDelay => (Flags & WorldItemSyncFlags.HasEnemyGrabDelay) != 0;

        public static bool IsSupported(PacketTypes packetType)
        {
            return packetType == PacketTypes.ItemDrop || packetType == PacketTypes.UpdateItemDrop;
        }
    }
}
