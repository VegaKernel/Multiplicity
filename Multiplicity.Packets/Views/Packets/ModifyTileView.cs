using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ModifyTileView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public ModifyTileView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.ModifyTile);

            PacketReader reader = packet.CreatePayloadReader();
            ModifyTileAction action = (ModifyTileAction)reader.ReadByte();
            short tileX = reader.ReadInt16();
            short tileY = reader.ReadInt16();
            short value = reader.ReadInt16();
            byte style = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            Action = action;
            TileX = tileX;
            TileY = tileY;
            Value = value;
            Style = style;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public ModifyTileAction Action { get; }

        public short TileX { get; }

        public short TileY { get; }

        public short Value { get; }

        public byte Style { get; }
    }
}
