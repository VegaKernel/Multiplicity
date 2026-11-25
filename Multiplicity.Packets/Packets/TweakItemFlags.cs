using System;

namespace Multiplicity.Packets
{
    [Flags]
    public enum TweakItemFlags1 : byte
    {
        None = 0,
        PackedColorValue = 1 << 0,
        Damage = 1 << 1,
        Knockback = 1 << 2,
        UseAnimation = 1 << 3,
        UseTime = 1 << 4,
        Shoot = 1 << 5,
        ShootSpeed = 1 << 6,
        SecondaryFlags = 1 << 7
    }

    [Flags]
    public enum TweakItemFlags2 : byte
    {
        None = 0,
        Scale = 1 << 2,
        Ammo = 1 << 3,
        UseAmmo = 1 << 4,
        NotAmmo = 1 << 5
    }
}
