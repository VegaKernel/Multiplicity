using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SyncItemDespawnView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public SyncItemDespawnView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.SyncItemDespawn);
                PacketReader reader = packet.CreatePayloadReader();
                short itemId = reader.ReadInt16();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                ItemId = itemId;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public short ItemId { get; }
        }
}

