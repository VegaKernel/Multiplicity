using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SpectatePlayerView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public SpectatePlayerView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.SpectatePlayer);
                PacketReader reader = packet.CreatePayloadReader();
                byte playerId = reader.ReadByte();
                short targetPlayerId = reader.ReadInt16();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                PlayerId = playerId;
                TargetPlayerId = targetPlayerId;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public byte PlayerId { get; }
            public short TargetPlayerId { get; }
        }
}

