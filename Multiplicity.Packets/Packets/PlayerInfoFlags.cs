using System;

namespace Multiplicity.Packets
{
    [Flags]
    public enum PlayerDifficultyFlags : byte
    {
        None = 0,
        Mediumcore = 1 << 0,
        Hardcore = 1 << 1,
        ExtraAccessory = 1 << 2,
        Creative = 1 << 3
    }

    [Flags]
    public enum PlayerTorchAndCartFlags : byte
    {
        None = 0,
        UsingBiomeTorches = 1 << 0,
        HappyFunTorchTime = 1 << 1,
        UnlockedBiomeTorches = 1 << 2,
        UnlockedSuperCart = 1 << 3,
        EnabledSuperCart = 1 << 4
    }

    [Flags]
    public enum PlayerConsumableUnlockFlags : byte
    {
        None = 0,
        UsedAegisCrystal = 1 << 0,
        UsedAegisFruit = 1 << 1,
        UsedArcaneCrystal = 1 << 2,
        UsedGalaxyPearl = 1 << 3,
        UsedGummyWorm = 1 << 4,
        UsedAmbrosia = 1 << 5,
        AteArtisanBread = 1 << 6
    }
}
