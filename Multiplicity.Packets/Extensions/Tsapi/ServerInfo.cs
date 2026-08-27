using System;
using System.IO;

namespace Multiplicity.Packets.Extensions.Tsapi
{
    /// <summary>
    /// TSAPI-family extension packet 163. This id is outside vanilla Terraria 1.4.5.8 (1..162).
    /// The payload remains opaque until a stable extension contract is pinned.
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
