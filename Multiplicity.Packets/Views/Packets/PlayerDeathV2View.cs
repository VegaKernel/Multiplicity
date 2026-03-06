using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerDeathV2View
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public PlayerDeathV2View(PacketView packet)
        {
            packet.EnsureType(PacketTypes.PlayerDeathV2);

            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            PlayerDeathReasonFlags deathReasonFlags = (PlayerDeathReasonFlags)reader.ReadByte();
            short fromPlayerIndex = PlayerDeathReasonView.MissingEntityIndex;
            short fromNpcIndex = PlayerDeathReasonView.MissingEntityIndex;
            short fromProjectileIndex = PlayerDeathReasonView.MissingEntityIndex;
            byte fromOther = PlayerDeathReasonView.NoOtherCause;
            short fromProjectileType = 0;
            short fromItemType = 0;
            byte fromItemPrefix = 0;
            ReadOnlySpan<byte> fromCustomReasonUtf8 = default;

            if ((deathReasonFlags & PlayerDeathReasonFlags.FromPlayerIndex) != 0)
            {
                fromPlayerIndex = reader.ReadInt16();
            }

            if ((deathReasonFlags & PlayerDeathReasonFlags.FromNpcIndex) != 0)
            {
                fromNpcIndex = reader.ReadInt16();
            }

            if ((deathReasonFlags & PlayerDeathReasonFlags.FromProjectileIndex) != 0)
            {
                fromProjectileIndex = reader.ReadInt16();
            }

            if ((deathReasonFlags & PlayerDeathReasonFlags.FromOther) != 0)
            {
                fromOther = reader.ReadByte();
            }

            if ((deathReasonFlags & PlayerDeathReasonFlags.FromProjectileType) != 0)
            {
                fromProjectileType = reader.ReadInt16();
            }

            if ((deathReasonFlags & PlayerDeathReasonFlags.FromItemType) != 0)
            {
                fromItemType = reader.ReadInt16();
            }

            if ((deathReasonFlags & PlayerDeathReasonFlags.FromItemPrefix) != 0)
            {
                fromItemPrefix = reader.ReadByte();
            }

            if ((deathReasonFlags & PlayerDeathReasonFlags.FromCustomReason) != 0)
            {
                fromCustomReasonUtf8 = reader.ReadLengthPrefixedBytes();
            }
            short damage = reader.ReadInt16();
            byte hitDirection = reader.ReadByte();
            byte flags = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            PlayerId = playerId;
            DeathReasonFlags = deathReasonFlags;
            FromPlayerIndex = fromPlayerIndex;
            FromNpcIndex = fromNpcIndex;
            FromProjectileIndex = fromProjectileIndex;
            FromOther = fromOther;
            FromProjectileType = fromProjectileType;
            FromItemType = fromItemType;
            FromItemPrefix = fromItemPrefix;
            FromCustomReasonUtf8 = fromCustomReasonUtf8;
            Damage = damage;
            HitDirection = hitDirection;
            Flags = flags;
        }

        public PacketHeader Header { get; }

        public ReadOnlySpan<byte> PacketSpan => _packetSpan;

        public byte PlayerId { get; }

        public PlayerDeathReasonFlags DeathReasonFlags { get; }

        public short FromPlayerIndex { get; }

        public short FromNpcIndex { get; }

        public short FromProjectileIndex { get; }

        public byte FromOther { get; }

        public short FromProjectileType { get; }

        public short FromItemType { get; }

        public byte FromItemPrefix { get; }

        public ReadOnlySpan<byte> FromCustomReasonUtf8 { get; }

        public short Damage { get; }

        public byte HitDirection { get; }

        public byte Flags { get; }

        public PlayerDeathReasonView GetDeathReason()
        {
            return new PlayerDeathReasonView(
                DeathReasonFlags,
                FromPlayerIndex,
                FromNpcIndex,
                FromProjectileIndex,
                FromOther,
                FromProjectileType,
                FromItemType,
                FromItemPrefix,
                FromCustomReasonUtf8);
        }
    }
}
