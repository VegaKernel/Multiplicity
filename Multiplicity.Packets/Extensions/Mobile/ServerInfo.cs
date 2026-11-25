using System;
using System.IO;

namespace Multiplicity.Packets.Extensions.Mobile
{
    /// <summary>
    /// Opaque legacy mobile-family packet 163; use the vanilla model for protocol 329.
    /// </summary>
    public sealed class ServerInfo : RawPayloadPacket
    {
        public const byte PacketId = 163;

        public ServerInfo(ReadOnlyMemory<byte> payload)
            : base(PacketId)
        {
            PayloadMemory = payload;
        }

        public ServerInfo(BinaryReader reader)
            : base(reader)
        {
        }
    }
}
