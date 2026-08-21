using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class SyncItemsWithShimmer : WorldItemSyncPacketBase
    {
        public SyncItemsWithShimmer()
            : base((byte)PacketTypes.SyncItemsWithShimmerDeprecated)
        {
        }

        public SyncItemsWithShimmer(BinaryReader br)
            : base(br)
        {
        }
    }
}
