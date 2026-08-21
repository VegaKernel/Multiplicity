namespace Multiplicity.Packets
{
    /// <summary>
    /// Terraria 1.4.5.7 / protocol 325 NetManager registration order.
    /// </summary>
    public enum NetworkModuleTypes : ushort
    {
        NetLiquidModule = 0,
        NetTextModule = 1,
        NetPingModule = 2,
        NetAmbienceModule = 3,
        NetBestiaryModule = 4,
        NetCreativeUnlocksModule = 5,
        NetCreativePowersModule = 6,
        NetCreativeUnlocksPlayerReportModule = 7,
        NetTeleportPylonModule = 8,
        NetParticlesModule = 9,
        NetCreativePowerPermissionsModule = 10,
        NetBannersModule = 11,
        NetCraftingRequestsModule = 12,
        LeashedEntityModule = 13,
        UnbreakableWallScanModule = 14
    }
}
