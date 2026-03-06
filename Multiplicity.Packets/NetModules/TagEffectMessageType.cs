namespace Multiplicity.Packets
{
    public enum TagEffectMessageType : byte
    {
        FullState = 0,
        ChangeActiveEffect = 1,
        ApplyTagToNpc = 2,
        EnableProcOnNpc = 3,
        ClearProcOnNpc = 4
    }
}