using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TileFrameSectionView
    {
        private readonly PacketView _packet;
        private readonly TileFrameSection _packetObject;

        public TileFrameSectionView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.TileFrameSection, payload))
        {
        }

        public static TileFrameSectionView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new TileFrameSectionView(payload);
        }

        public TileFrameSectionView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<TileFrameSection>(packet, PacketTypes.TileFrameSection);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public TileFrameSection Packet => _packetObject;

        public short StartX => _packetObject.StartX;

        public short StartY => _packetObject.StartY;

        public short EndX => _packetObject.EndX;

        public short EndY => _packetObject.EndY;
    }
}


