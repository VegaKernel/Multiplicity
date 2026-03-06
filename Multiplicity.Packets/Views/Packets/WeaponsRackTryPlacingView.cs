using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct WeaponsRackTryPlacingView
    {
        private readonly TileEntityItemSyncPacketView _view;

        public WeaponsRackTryPlacingView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.WeaponsRackTryPlacing, payload))
        {
        }

        public static WeaponsRackTryPlacingView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new WeaponsRackTryPlacingView(payload);
        }

        public WeaponsRackTryPlacingView(PacketView packet)
        {
            _view = new TileEntityItemSyncPacketView(packet, PacketTypes.WeaponsRackTryPlacing);
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
