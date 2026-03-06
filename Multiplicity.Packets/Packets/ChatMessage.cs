using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The ChatMessage (0x19) packet.
    /// </summary>
    public class ChatMessage : TerrariaPacket
    {
        private const short FixedPayloadLength = 4;

        public byte PlayerId { get; set; }
        public ColorStruct MessageColor { get; set; }
        public string Message { get; set; }

        public ChatMessage()
            : base((byte)PacketTypes.ChatMessage)
        {
            Message = string.Empty;
        }

        public ChatMessage(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            MessageColor = br.ReadColor();
            Message = br.ReadString();
        }

        public override short GetLength()
        {
            return (short)(FixedPayloadLength + PacketSerializationHelper.GetSerializedStringLength(Message));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(PlayerId);
                bw.Write(MessageColor);
                bw.Write(Message ?? string.Empty);
            }
        }

        public override string ToString()
        {
            return $"[ChatMessage: PlayerId = {PlayerId} MessageColor = {MessageColor} Message = {Message}]";
        }
    }
}

