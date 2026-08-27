using System;
using System.IO;

namespace Multiplicity.Packets
{
    [PacketRegistrationIgnore]
    public sealed class SyncItemsWithShimmer : WorldItemSyncPacketBase
    {
        public SyncItemsWithShimmer()
            : base((byte)PacketTypes.SyncItemsWithShimmerDeprecated)
        {
            throw new NotSupportedException("Packet 145 is deprecated and cannot be sent in Terraria protocol 325.");
        }

        public SyncItemsWithShimmer(BinaryReader br)
            : base(br)
        {
            throw new InvalidDataException("Packet 145 is deprecated in Terraria protocol 325.");
        }
    }
}
