using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct RequestTileEntityInteractionView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public RequestTileEntityInteractionView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.RequestTileEntityInteraction);
                PacketReader reader = packet.CreatePayloadReader();
                int tileEntityId = reader.ReadInt32();
                byte playerId = reader.ReadByte();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                TileEntityId = tileEntityId;
                PlayerId = playerId;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public int TileEntityId { get; }
            public byte PlayerId { get; }
        }
}

