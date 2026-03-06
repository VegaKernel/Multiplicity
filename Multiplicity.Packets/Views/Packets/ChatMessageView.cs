using System;
using System.Text;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ChatMessageView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public ChatMessageView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.ChatMessage);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            ColorStruct messageColor = reader.ReadColor();
            ReadOnlySpan<byte> messageUtf8 = reader.ReadLengthPrefixedBytes();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            PlayerId = playerId;
            MessageColor = messageColor;
            MessageUtf8 = messageUtf8;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public byte PlayerId { get; }

        public ColorStruct MessageColor { get; }

        public ReadOnlySpan<byte> MessageUtf8 { get; }

        public string GetMessage()
        {
            return Encoding.UTF8.GetString(MessageUtf8);
        }
    }
}
