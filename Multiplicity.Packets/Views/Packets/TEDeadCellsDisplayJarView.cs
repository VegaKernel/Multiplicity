using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TEDeadCellsDisplayJarView
    {
        private readonly TileEntityItemSyncPacketView _view;

        public TEDeadCellsDisplayJarView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.TEDeadCellsDisplayJar, payload))
        {
        }

        public static TEDeadCellsDisplayJarView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new TEDeadCellsDisplayJarView(payload);
        }

        public TEDeadCellsDisplayJarView(PacketView packet)
        {
            _view = new TileEntityItemSyncPacketView(packet, PacketTypes.TEDeadCellsDisplayJar);
        }

        public PacketHeader Header => _view.Header;
        public ReadOnlySpan<byte> SourceSpan => _view.SourceSpan;
        public short TileX => _view.TileX;
        public short TileY => _view.TileY;
        public short ItemType => _view.ItemType;
        public byte Prefix => _view.Prefix;
        public short Stack => _view.Stack;
    }
}
