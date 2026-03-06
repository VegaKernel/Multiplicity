namespace Multiplicity.Packets
{
    public enum NetBannersMessageType : byte
    {
        FullState = 0,
        KillCountUpdate = 1,
        ClaimCountUpdate = 2,
        ClaimRequest = 3,
        ClaimResponse = 4
    }
}