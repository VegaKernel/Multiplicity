using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TileView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public TileView(ReadOnlySpan<byte> payload)
        : this(PacketView.FromPayload(PacketTypes.Tile, payload))
        {
        }

        public static TileView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new TileView(payload);
        }

        public TileView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.Tile);

            PacketReader reader = packet.CreatePayloadReader();
            TileAction action = (TileAction)reader.ReadByte();
            short tileX = reader.ReadInt16();
            short tileY = reader.ReadInt16();
            short value = reader.ReadInt16();
            byte style = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            Action = action;
            TileX = tileX;
            TileY = tileY;
            Value = value;
            Style = style;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;

        public TileAction Action { get; }

        public short TileX { get; }

        public short TileY { get; }

        public short Value { get; }

        public byte Style { get; }
    }
}



