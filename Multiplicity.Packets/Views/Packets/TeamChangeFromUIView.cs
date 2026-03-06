using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TeamChangeFromUIView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public TeamChangeFromUIView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.TeamChangeFromUI);
                PacketReader reader = packet.CreatePayloadReader();
                byte playerId = reader.ReadByte();
                byte team = reader.ReadByte();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                PlayerId = playerId;
                Team = team;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public byte PlayerId { get; }
            public byte Team { get; }
        }
}

