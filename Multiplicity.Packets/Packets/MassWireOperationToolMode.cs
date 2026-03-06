using System;

namespace Multiplicity.Packets
{
    [Flags]
    public enum MassWireOperationToolMode : byte
    {
        Red = 1,
        Green = 2,
        Blue = 4,
        Yellow = 8,
        Actuator = 16,
        Cutter = 32
    }
}
