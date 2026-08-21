using System;

namespace Multiplicity.Packets
{
    public enum NewItemOwnership : byte
    {
        None = 0,
        ReserveForLocalPlayer = 1,
        GrabDelayForLocalPlayer = 2,
        GrabDelayForAllPlayers = 3
    }

    [Flags]
    public enum WorldItemSyncFlags : byte
    {
        None = 0,
        OwnershipBit0 = 1 << 0,
        OwnershipBit1 = 1 << 1,
        OwnershipMask = OwnershipBit0 | OwnershipBit1,
        HasShimmerData = 1 << 2,
        HasEnemyGrabDelay = 1 << 3
    }
}
