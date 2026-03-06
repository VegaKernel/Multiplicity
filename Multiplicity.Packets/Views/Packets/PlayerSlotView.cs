using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerSlotView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public PlayerSlotView(ReadOnlySpan<byte> payload)
        : this(PacketView.FromPayload(PacketTypes.PlayerSlot, payload))
        {
        }

        public static PlayerSlotView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlayerSlotView(payload);
        }

        public PlayerSlotView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlayerSlot);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            short slotId = reader.ReadInt16();
            short stack = reader.ReadInt16();
            byte prefix = reader.ReadByte();
            short itemNetId = reader.ReadInt16();
            byte itemFlags = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            SlotId = slotId;
            Stack = stack;
            Prefix = prefix;
            ItemNetId = itemNetId;
            ItemFlags = itemFlags;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;

        public byte PlayerId { get; }

        public short SlotId { get; }

        public short Stack { get; }

        public byte Prefix { get; }

        public short ItemNetId { get; }

        public byte ItemFlags { get; }
    }
}



