using System;
using System.Text;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ChatMessageView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public ChatMessageView(ReadOnlySpan<byte> payload)
        : this(PacketView.FromPayload(PacketTypes.ChatMessage, payload))
        {
        }

        public static ChatMessageView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ChatMessageView(payload);
        }

        public ChatMessageView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.ChatMessage);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            ColorStruct messageColor = reader.ReadColor();
            ReadOnlySpan<byte> messageUtf8 = reader.ReadLengthPrefixedBytes();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            MessageColor = messageColor;
            MessageUtf8 = messageUtf8;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;

        public byte PlayerId { get; }

        public ColorStruct MessageColor { get; }

        public ReadOnlySpan<byte> MessageUtf8 { get; }

        public string GetMessage()
        {
            return Encoding.UTF8.GetString(MessageUtf8);
        }
    }
}



