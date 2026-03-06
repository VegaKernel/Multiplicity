using System;

namespace Multiplicity.Packets
{
    [Flags]
    public enum PlayerDeathReasonFlags : byte
    {
        None = 0,
        FromPlayerIndex = 1 << 0,
        FromNpcIndex = 1 << 1,
        FromProjectileIndex = 1 << 2,
        FromOther = 1 << 3,
        FromProjectileType = 1 << 4,
        FromItemType = 1 << 5,
        FromItemPrefix = 1 << 6,
        FromCustomReason = 1 << 7
    }
}
