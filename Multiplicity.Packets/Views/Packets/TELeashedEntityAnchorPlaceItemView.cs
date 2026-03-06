using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TELeashedEntityAnchorPlaceItemView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public TELeashedEntityAnchorPlaceItemView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.TELeashedEntityAnchorPlaceItem);
                PacketReader reader = packet.CreatePayloadReader();
                short tileX = reader.ReadInt16();
                short tileY = reader.ReadInt16();
                short itemType = reader.ReadInt16();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                TileX = tileX;
                TileY = tileY;
                ItemType = itemType;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public short TileX { get; }
            public short TileY { get; }
            public short ItemType { get; }
        }
}

