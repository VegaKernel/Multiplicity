using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct FoodPlatterTryPlacingView
    {
        private readonly TileEntityItemSyncPacketView _view;

        public FoodPlatterTryPlacingView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.FoodPlatterTryPlacing, payload))
        {
        }

        public static FoodPlatterTryPlacingView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new FoodPlatterTryPlacingView(payload);
        }

        public FoodPlatterTryPlacingView(PacketView packet)
        {
            _view = new TileEntityItemSyncPacketView(packet, PacketTypes.FoodPlatterTryPlacing);
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
