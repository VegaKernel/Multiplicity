namespace Multiplicity.Packets
{
    public enum EmoteBubbleAnchorType : byte
    {
        Npc = 0,
        Player = 1,
        Projectile = 2,
        None = 3,
        Removed = byte.MaxValue
    }
}
