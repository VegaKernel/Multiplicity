using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TileSendSectionView
    {
        private readonly PacketView _packet;
        private readonly TileSendSection _packetObject;

        public TileSendSectionView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.TileSendSection, payload))
        {
        }

        public static TileSendSectionView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new TileSendSectionView(payload);
        }

        public TileSendSectionView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<TileSendSection>(packet, PacketTypes.TileSendSection);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public TileSendSection Packet => _packetObject;

        public ReadOnlyMemory<byte> CompressedTileBlockMemory => _packetObject.CompressedTileBlockMemory;
    }
}


