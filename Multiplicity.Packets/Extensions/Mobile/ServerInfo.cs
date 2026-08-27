using System;
using System.IO;

namespace Multiplicity.Packets.Extensions.Mobile
{
    /// <summary>
    /// Mobile-family extension packet 163. This id is outside the vanilla Terraria 1.4.5.8 desktop range (0..162).
    /// The payload remains opaque until the exact 163 wire contract is pinned against a matching mobile build.
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
