using System;

namespace Multiplicity.Packets
{
    [Flags]
    public enum NpcShopItemFlags : byte
    {
        None = 0,
        BuyOnce = 1
    }
}
