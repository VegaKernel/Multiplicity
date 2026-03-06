using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PaintWallView
    {
        private readonly PacketView _packet;
        private readonly PaintWall _packetObject;

        public PaintWallView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.PaintWall, payload))
        {
        }

        public static PaintWallView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new PaintWallView(payload);
        }

        public PaintWallView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<PaintWall>(packet, PacketTypes.PaintWall);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public PaintWall Packet => _packetObject;

        public short X => _packetObject.X;

        public short Y => _packetObject.Y;

        public byte Color => _packetObject.Color;
    }
}


