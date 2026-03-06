namespace Multiplicity.Packets
{
    public enum LeashedEntityMessageType : byte
    {
        Remove = 0,
        FullSync = 1,
        PartialSync = 2
    }
}