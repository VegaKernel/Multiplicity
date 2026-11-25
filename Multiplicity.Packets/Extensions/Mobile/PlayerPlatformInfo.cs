using System;
using System.IO;

namespace Multiplicity.Packets.Extensions.Mobile
{
    /// <summary>
    /// Opaque legacy mobile-family packet 164; use the vanilla model for protocol 329.
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
