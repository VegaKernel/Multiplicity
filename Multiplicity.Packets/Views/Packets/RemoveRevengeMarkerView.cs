using System;
using System.Buffers.Binary;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct RemoveRevengeMarkerView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public RemoveRevengeMarkerView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.RemoveRevengeMarker);
                PacketReader reader = packet.CreatePayloadReader();
                int uniqueId = reader.ReadInt32();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                UniqueId = uniqueId;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public int UniqueId { get; }
        }
}

