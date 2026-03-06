using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SetCountsAsHostForGameplayView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public SetCountsAsHostForGameplayView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.SetCountsAsHostForGameplay);
                PacketReader reader = packet.CreatePayloadReader();
                byte playerId = reader.ReadByte();
                bool countsAsHostForGameplay = reader.ReadBoolean();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                PlayerId = playerId;
                CountsAsHostForGameplay = countsAsHostForGameplay;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public byte PlayerId { get; }
            public bool CountsAsHostForGameplay { get; }
        }
}

