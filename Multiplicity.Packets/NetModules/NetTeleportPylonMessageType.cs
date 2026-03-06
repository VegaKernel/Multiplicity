namespace Multiplicity.Packets
{
    public enum NetTeleportPylonMessageType : byte
    {
        PylonWasAdded = 0,
        PylonWasRemoved = 1,
        PlayerRequestsTeleport = 2
    }
}