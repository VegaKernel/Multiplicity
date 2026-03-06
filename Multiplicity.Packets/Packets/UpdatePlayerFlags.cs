using System;

namespace Multiplicity.Packets
{
    [Flags]
    public enum UpdatePlayerControlFlags : byte
    {
        None = 0,
        ControlUp = 1 << 0,
        ControlDown = 1 << 1,
        ControlLeft = 1 << 2,
        ControlRight = 1 << 3,
        ControlJump = 1 << 4,
        ControlUseItem = 1 << 5,
        DirectionRight = 1 << 6
    }

    [Flags]
    public enum UpdatePlayerMovementFlags : byte
    {
        None = 0,
        IsUsingPulley = 1 << 0,
        PulleyDirectionIsReverse = 1 << 1,
        HasVelocity = 1 << 2,
        VortexStealthActive = 1 << 3,
        GravityDirectionIsInverted = 1 << 4,
        ShieldRaised = 1 << 5,
        IsGhost = 1 << 6,
        HasMount = 1 << 7
    }

    [Flags]
    public enum UpdatePlayerMiscFlags1 : byte
    {
        None = 0,
        KeepHoveringUp = 1 << 0,
        VoidVaultEnabled = 1 << 1,
        IsSitting = 1 << 2,
        DownedDd2EventAnyDifficulty = 1 << 3,
        IsPetting = 1 << 4,
        IsPetSmall = 1 << 5,
        HasPotionOfReturnPositions = 1 << 6,
        KeepHoveringDown = 1 << 7
    }

    [Flags]
    public enum UpdatePlayerMiscFlags2 : byte
    {
        None = 0,
        IsSleeping = 1 << 0,
        AutoReuseAllWeapons = 1 << 1,
        ControlDownHold = 1 << 2,
        IsOperatingAnotherEntity = 1 << 3,
        ControlUseTile = 1 << 4,
        HasCameraTarget = 1 << 5,
        LastItemUseAttemptSuccess = 1 << 6
    }
}
