using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct RequestSectionView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public RequestSectionView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.RequestSection);
                PacketReader reader = packet.CreatePayloadReader();
                ushort sectionX = reader.ReadUInt16();
                ushort sectionY = reader.ReadUInt16();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                SectionX = sectionX;
                SectionY = sectionY;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public ushort SectionX { get; }
            public ushort SectionY { get; }
        }
}

