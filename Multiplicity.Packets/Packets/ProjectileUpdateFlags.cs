using System;

namespace Multiplicity.Packets
{
    [Flags]
    public enum ProjectileUpdateFlags : byte
    {
        None = 0,
        HasAI0 = 1 << 0,
        HasAI1 = 1 << 1,
        HasExtraFlags = 1 << 2,
        HasBannerIdToRespondTo = 1 << 3,
        HasDamage = 1 << 4,
        HasKnockBack = 1 << 5,
        HasOriginalDamage = 1 << 6,
        HasProjectileUuid = 1 << 7
    }

    [Flags]
    public enum ProjectileUpdateExtraFlags : byte
    {
        None = 0,
        HasAI2 = 1 << 0
    }
}
