using System;
using System.IO;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct WorldItemSyncView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public WorldItemSyncView(PacketView packet)
        {
            if (!IsSupported(packet.PacketType))
            {
                throw new InvalidDataException($"Packet type {packet.PacketType} is not a supported world item sync packet.");
            }

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
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            ItemIndex = itemIndex;
            PositionX = positionX;
            PositionY = positionY;
            VelocityX = velocityX;
            VelocityY = velocityY;
            Stack = stack;
            Prefix = prefix;
            Flags = flags;
            ItemNetId = itemNetId;
        }

        public PacketHeader Header { get; }

        public PacketTypes PacketType => Header.PacketType;

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public short ItemIndex { get; }

        public float PositionX { get; }

        public float PositionY { get; }

        public float VelocityX { get; }

        public float VelocityY { get; }

        public short Stack { get; }

        public byte Prefix { get; }

        public WorldItemSyncFlags Flags { get; }

        public short ItemNetId { get; }

        public static bool IsSupported(PacketTypes packetType)
        {
            return packetType == PacketTypes.UpdateItemDrop
                || packetType == PacketTypes.UpdateItemDrop2
                || packetType == PacketTypes.SyncItemsWithShimmer
                || packetType == PacketTypes.SyncItemCannotBeTakenByEnemies;
        }
    }
}
