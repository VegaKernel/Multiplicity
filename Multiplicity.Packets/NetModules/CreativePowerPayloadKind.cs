namespace Multiplicity.Packets
{
    public enum CreativePowerPayloadKind : byte
    {
        Raw = 0,
        SharedButton = 1,
        SharedToggle = 2,
        SharedSlider = 3,
        PerPlayerSlider = 4,
        PerPlayerToggle = 5
    }
}
