using System;

namespace Multiplicity.Packets
{
    [Flags]
    public enum WorldItemSyncFlags : byte
    {
        None = 0,
        Reserved0 = 1 << 0,
        KeepTime = 1 << 1
    }
}
