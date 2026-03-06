using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class ServerInfo : RawPayloadPacket
        {
            public ServerInfo()
                : base((byte)PacketTypes.ServerInfo)
            {
            }
    
            public ServerInfo(BinaryReader br)
                : base(br)
            {
            }
        }
}

