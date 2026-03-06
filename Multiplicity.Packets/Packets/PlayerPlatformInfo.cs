using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class PlayerPlatformInfo : RawPayloadPacket
        {
            public PlayerPlatformInfo()
                : base((byte)PacketTypes.PlayerPlatformInfo)
            {
            }
    
            public PlayerPlatformInfo(BinaryReader br)
                : base(br)
            {
            }
        }
}

