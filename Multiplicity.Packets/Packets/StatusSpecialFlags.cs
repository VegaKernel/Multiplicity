using System;

namespace Multiplicity.Packets
{
    [Flags]
    public enum StatusSpecialFlags : byte
    {
        None = 0,
        HideStatusTextPercent = 1 << 0,
        StatusTextHasShadows = 1 << 1,
        RunCheckBytesInClientLoopThread = 1 << 2
    }
}
