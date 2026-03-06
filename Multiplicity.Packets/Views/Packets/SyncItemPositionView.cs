using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SyncItemPositionView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public SyncItemPositionView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.SyncItemPosition);
                PacketReader reader = packet.CreatePayloadReader();
                short itemId = reader.ReadInt16();
                float positionX = reader.ReadSingle();
                float positionY = reader.ReadSingle();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                ItemId = itemId;
                PositionX = positionX;
                PositionY = positionY;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public short ItemId { get; }
            public float PositionX { get; }
            public float PositionY { get; }
        }
}

