using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ChestGetContentsView
    {
        private readonly PacketView _packet;
        private readonly ChestGetContents _packetObject;

        public ChestGetContentsView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ChestGetContents, payload))
        {
        }

        public static ChestGetContentsView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ChestGetContentsView(payload);
        }

        public ChestGetContentsView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<ChestGetContents>(packet, PacketTypes.ChestGetContents);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public ChestGetContents Packet => _packetObject;

        public short TileX => _packetObject.TileX;

        public short TileY => _packetObject.TileY;
    }
}


