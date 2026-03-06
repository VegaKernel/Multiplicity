namespace Multiplicity.Packets.Views
{
    public static class NetModuleViewExtensions
    {
        public static NetLiquidModuleView AsNetLiquidModuleView(this LoadNetModuleView module) => new NetLiquidModuleView(module);
        public static NetTextModuleView AsNetTextModuleView(this LoadNetModuleView module) => new NetTextModuleView(module);
        public static NetPingModuleView AsNetPingModuleView(this LoadNetModuleView module) => new NetPingModuleView(module);
        public static NetAmbienceModuleView AsNetAmbienceModuleView(this LoadNetModuleView module) => new NetAmbienceModuleView(module);
        public static NetBestiaryModuleView AsNetBestiaryModuleView(this LoadNetModuleView module) => new NetBestiaryModuleView(module);
        public static NetCreativePowersModuleView AsNetCreativePowersModuleView(this LoadNetModuleView module) => new NetCreativePowersModuleView(module);
        public static NetCreativeUnlocksPlayerReportModuleView AsNetCreativeUnlocksPlayerReportModuleView(this LoadNetModuleView module) => new NetCreativeUnlocksPlayerReportModuleView(module);
        public static NetTeleportPylonModuleView AsNetTeleportPylonModuleView(this LoadNetModuleView module) => new NetTeleportPylonModuleView(module);
        public static NetParticlesModuleView AsNetParticlesModuleView(this LoadNetModuleView module) => new NetParticlesModuleView(module);
        public static NetCreativePowerPermissionsModuleView AsNetCreativePowerPermissionsModuleView(this LoadNetModuleView module) => new NetCreativePowerPermissionsModuleView(module);
        public static NetBannersModuleView AsNetBannersModuleView(this LoadNetModuleView module) => new NetBannersModuleView(module);
        public static NetCraftingRequestsModuleView AsNetCraftingRequestsModuleView(this LoadNetModuleView module) => new NetCraftingRequestsModuleView(module);
        public static TagEffectModuleView AsTagEffectModuleView(this LoadNetModuleView module) => new TagEffectModuleView(module);
        public static LeashedEntityModuleView AsLeashedEntityModuleView(this LoadNetModuleView module) => new LeashedEntityModuleView(module);
        public static UnbreakableWallScanModuleView AsUnbreakableWallScanModuleView(this LoadNetModuleView module) => new UnbreakableWallScanModuleView(module);
    }
}