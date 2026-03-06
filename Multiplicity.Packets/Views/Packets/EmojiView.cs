using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct EmojiView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public EmojiView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.Emoji);
                PacketReader reader = packet.CreatePayloadReader();
                byte playerId = reader.ReadByte();
                byte emoteId = reader.ReadByte();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                PlayerId = playerId;
                EmoteId = emoteId;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public byte PlayerId { get; }
            public byte EmoteId { get; }
        }
}

