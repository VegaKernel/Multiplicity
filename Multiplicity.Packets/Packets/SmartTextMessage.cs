using System.IO;
using Multiplicity.Packets.Extensions;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The SmartTextMessage (0x6B) packet.
    /// </summary>
    public class SmartTextMessage : TerrariaPacket
    {
        private const short FixedPayloadLength = 5;

        public ColorStruct MessageColor { get; set; }
        public NetworkText Message { get; set; }
        public short WidthLimit { get; set; }

        public SmartTextMessage()
            : base((byte)PacketTypes.SmartTextMessage)
        {
            Message = new NetworkText();
        }

        public SmartTextMessage(BinaryReader br)
            : base(br)
        {
            MessageColor = br.ReadColor();
            Message = br.ReadNetworkText();
            WidthLimit = br.ReadInt16();
        }

        public override short GetLength()
        {
            return (short)(FixedPayloadLength + (Message?.GetLength() ?? 0));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(MessageColor);
                bw.Write(Message ?? new NetworkText());
                bw.Write(WidthLimit);
            }
        }

        public override string ToString()
        {
            return $"[SmartTextMessage: MessageColor = {MessageColor} Message = {Message?.Text} WidthLimit = {WidthLimit}]";
        }
    }
}

