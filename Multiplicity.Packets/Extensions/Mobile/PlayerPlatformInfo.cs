using System;
using System.IO;

namespace Multiplicity.Packets.Extensions.Mobile
{
    /// <summary>
    /// Mobile-family extension packet 164. This id is outside the vanilla Terraria 1.4.5.8 desktop range (0..162).
    /// The payload remains opaque until the exact 164 wire contract is pinned against a matching mobile build.
    /// </summary>
    public sealed class PlayerPlatformInfo : RawPayloadPacket
    {
        public const byte PacketId = 164;

        public PlayerPlatformInfo(ReadOnlyMemory<byte> payload)
            : base(PacketId)
        {
            PayloadMemory = payload;
        }

        public PlayerPlatformInfo(BinaryReader reader)
            : base(reader)
        {
        }
    }
}
