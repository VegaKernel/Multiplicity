using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class FoodPlatterTryPlacing : TileEntityItemSyncPacketBase
        {
            public FoodPlatterTryPlacing()
                : base((byte)PacketTypes.FoodPlatterTryPlacing)
            {
            }
    
            public FoodPlatterTryPlacing(BinaryReader br)
                : base(br)
            {
            }
        }
}

