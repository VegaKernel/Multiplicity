using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ExtraSpawnSectionLoadedView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public ExtraSpawnSectionLoadedView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.ExtraSpawnSectionLoaded);
                PacketReader reader = packet.CreatePayloadReader();
                byte playerId = reader.ReadByte();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                PlayerId = playerId;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public byte PlayerId { get; }
        }
}

