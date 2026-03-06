using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerInventorySlotView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public PlayerInventorySlotView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlayerInventorySlot);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            short slotId = reader.ReadInt16();
            short stack = reader.ReadInt16();
            byte prefix = reader.ReadByte();
            short itemNetId = reader.ReadInt16();
            byte itemFlags = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            PlayerId = playerId;
            SlotId = slotId;
            Stack = stack;
            Prefix = prefix;
            ItemNetId = itemNetId;
            ItemFlags = itemFlags;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public byte PlayerId { get; }

        public short SlotId { get; }

        public short Stack { get; }

        public byte Prefix { get; }

        public short ItemNetId { get; }

        public byte ItemFlags { get; }
    }
}
