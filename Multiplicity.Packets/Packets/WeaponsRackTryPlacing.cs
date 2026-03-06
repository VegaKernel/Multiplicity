using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class WeaponsRackTryPlacing : TileEntityItemSyncPacketBase
        {
            public WeaponsRackTryPlacing()
                : base((byte)PacketTypes.WeaponsRackTryPlacing)
            {
            }
    
            public WeaponsRackTryPlacing(BinaryReader br)
                : base(br)
            {
            }
        }
}

