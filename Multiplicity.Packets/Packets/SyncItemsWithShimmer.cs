using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class SyncItemsWithShimmer : WorldItemSyncPacketBase
        {
            public SyncItemsWithShimmer()
                : base((byte)PacketTypes.SyncItemsWithShimmer)
            {
            }
    
            public SyncItemsWithShimmer(BinaryReader br)
                : base(br)
            {
            }
        }
}

