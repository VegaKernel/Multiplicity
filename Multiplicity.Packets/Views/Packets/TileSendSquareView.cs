using System;
using Multiplicity.Packets.Models;
using TileModel = Multiplicity.Packets.Models.Tile;

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

        public PacketReader CreateTilePayloadReader() => new PacketReader(TilePayloadMemory.Span);

        public TileModel[,] ReadTiles(Func<ushort, bool> isFrameImportant)
        {
            PacketReader reader = CreateTilePayloadReader();
            TileModel[,] tiles = TilePayloadReader.ReadRectangle(ref reader, Width, Height, isFrameImportant);
            reader.EnsureEnd();
            return tiles;
        }
    }
}
