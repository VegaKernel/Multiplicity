using System.IO;

namespace Multiplicity.Packets.Extensions.Tsapi
{
    /// <summary>
    /// TSAPI-family extension packet 164. This id is outside vanilla Terraria 1.4.5.8 (1..162).
    /// The payload remains opaque until a stable extension contract is pinned.
    /// </summary>
    public sealed class PlayerPlatformInfo : RawPayloadPacket
    {
        public const byte PacketId = 164;

        public PlayerPlatformInfo()
            : base(PacketId)
        {
        }

        public PlayerPlatformInfo(BinaryReader reader)
            : base(reader)
        {
        }
    }
}
