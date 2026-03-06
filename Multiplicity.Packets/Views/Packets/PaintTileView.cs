using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PaintTileView
    {
        private readonly PacketView _packet;
        private readonly PaintTile _packetObject;

        public PaintTileView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.PaintTile, payload))
        {
        }

        public static PaintTileView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PaintTileView(payload);
        }

        public PaintTileView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<PaintTile>(packet, PacketTypes.PaintTile);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public PaintTile Packet => _packetObject;

        public short X => _packetObject.X;

        public short Y => _packetObject.Y;

        public byte Color => _packetObject.Color;
    }
}


