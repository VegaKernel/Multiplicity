namespace Multiplicity.Packets
{
    /// <summary>
    /// Terraria 1.4.5.8 / protocol 326 LeashedEntity.NetModule registration order.
    /// Values are wire identifiers and must stay aligned with Terraria's registration sequence.
    /// </summary>
    public enum LeashedEntityType
    {
        None = 0,
        Kite = 1,
        WalkerCritter = 2,
        AquaticWalkerCritter = 3,
        CrawlerCritter = 4,
        SnailCritter = 5,
        TurtleCritter = 6,
        RunnerCritter = 7,
        FlyerCritter = 8,
        NormalButterfly = 9,
        EmpressButterfly = 10,
        HellButterfly = 11,
        Firefly = 12,
        ShimmerFly = 13,
        Dragonfly = 14,
        CrawlingFly = 15,
        Bird = 16,
        Waterfowl = 17,
        Fish = 18,
        Fairy = 19,
        Jumper = 20,
        WaterStrider = 21
    }
}
