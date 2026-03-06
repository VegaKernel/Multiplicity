using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlaceItemFrameView
    {
        private readonly PacketView _packet;
        private readonly PlaceItemFrame _packetObject;

        public PlaceItemFrameView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.PlaceItemFrame, payload))
        {
        }

        public static PlaceItemFrameView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PlaceItemFrameView(payload);
        }

        public PlaceItemFrameView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<PlaceItemFrame>(packet, PacketTypes.PlaceItemFrame);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public PlaceItemFrame Packet => _packetObject;

        public short X => _packetObject.X;

        public short Y => _packetObject.Y;

        public short ItemId => _packetObject.ItemId;

        public byte Prefix => _packetObject.Prefix;

        public short Stack => _packetObject.Stack;
    }
}


