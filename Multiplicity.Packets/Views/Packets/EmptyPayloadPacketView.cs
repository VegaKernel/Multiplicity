using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct EmptyPayloadPacketView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public EmptyPayloadPacketView(PacketView packet, PacketTypes expectedPacketType)
            {
                packet.EnsureType(expectedPacketType);
                PacketReader reader = packet.CreatePayloadReader();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                PacketType = expectedPacketType;
            }
    
            public PacketHeader Header { get; }
            public PacketTypes PacketType { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        }
}

