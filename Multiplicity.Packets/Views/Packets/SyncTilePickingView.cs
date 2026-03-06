using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SyncTilePickingView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public SyncTilePickingView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.SyncTilePicking);
                PacketReader reader = packet.CreatePayloadReader();
                byte playerId = reader.ReadByte();
                short tileX = reader.ReadInt16();
                short tileY = reader.ReadInt16();
                byte pickPower = reader.ReadByte();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                PlayerId = playerId;
                TileX = tileX;
                TileY = tileY;
                PickPower = pickPower;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public byte PlayerId { get; }
            public short TileX { get; }
            public short TileY { get; }
            public byte PickPower { get; }
        }
}

