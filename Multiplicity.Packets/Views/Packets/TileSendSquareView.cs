using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TileSendSquareView
    {
        private readonly PacketView _packet;
        private readonly TileSendSquare _packetObject;

        public TileSendSquareView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.TileSendSquare, payload))
        {
        }

        public static TileSendSquareView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new TileSendSquareView(payload);
        }

        public TileSendSquareView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<TileSendSquare>(packet, PacketTypes.TileSendSquare);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public TileSendSquare Packet => _packetObject;

        public short TileX => _packetObject.TileX;

        public short TileY => _packetObject.TileY;

        public byte Width => _packetObject.Width;

        public byte Height => _packetObject.Height;

        public byte TileChangeType => _packetObject.TileChangeType;

        public ReadOnlyMemory<byte> TilePayloadMemory => _packetObject.TilePayloadMemory;
    }
}


