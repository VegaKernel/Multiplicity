using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct RequestLucyPopupView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public RequestLucyPopupView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.RequestLucyPopup);
                PacketReader reader = packet.CreatePayloadReader();
                LucyPopupMessageSource messageSource = (LucyPopupMessageSource)reader.ReadByte();
                byte variation = reader.ReadByte();
                float velocityX = reader.ReadSingle();
                float velocityY = reader.ReadSingle();
                int positionX = reader.ReadInt32();
                int positionY = reader.ReadInt32();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                MessageSource = messageSource;
                Variation = variation;
                VelocityX = velocityX;
                VelocityY = velocityY;
                PositionX = positionX;
                PositionY = positionY;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public LucyPopupMessageSource MessageSource { get; }
            public byte Variation { get; }
            public float VelocityX { get; }
            public float VelocityY { get; }
            public int PositionX { get; }
            public int PositionY { get; }
        }
}

