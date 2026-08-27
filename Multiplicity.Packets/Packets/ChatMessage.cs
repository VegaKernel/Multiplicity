using System;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// Packet id 25 is unused in Terraria 1.4.5.x. Chat synchronization uses NetTextModule.
    /// This legacy type is retained only for source compatibility and must not be serialized.
    /// </summary>
    [PacketRegistrationIgnore]
    public class ChatMessage : TerrariaPacket
    {
        public byte PlayerId { get; set; }
        public ColorStruct MessageColor { get; set; }
        public string Message { get; set; } = string.Empty;

        public ChatMessage()
            : base((byte)PacketTypes.ChatMessage)
        {
            throw new NotSupportedException("Packet 25 is unused in Terraria protocol 326; chat uses NetTextModule.");
        }

        public ChatMessage(BinaryReader br)
            : base(br)
        {
            throw new InvalidDataException("Packet 25 is unused in Terraria protocol 326.");
        }

        public override short GetLength()
        {
            return 0;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            throw new NotSupportedException("Packet 25 cannot be sent in Terraria protocol 326; chat uses NetTextModule.");
        }

        public override string ToString()
        {
            return "[Unused25: unsupported in protocol 326]";
        }
    }
}
