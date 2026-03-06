using System;
using System.Text;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct PlayerDeathReasonView
    {
        public const short MissingEntityIndex = -1;
        public const byte NoOtherCause = 254;

        public PlayerDeathReasonView(
            PlayerDeathReasonFlags flags,
            short fromPlayerIndex,
            short fromNpcIndex,
            short fromProjectileIndex,
            byte fromOther,
            short fromProjectileType,
            short fromItemType,
            byte fromItemPrefix,
            ReadOnlySpan<byte> fromCustomReasonUtf8)
        {
            Flags = flags;
            FromPlayerIndex = fromPlayerIndex;
            FromNpcIndex = fromNpcIndex;
            FromProjectileIndex = fromProjectileIndex;
            FromOther = fromOther;
            FromProjectileType = fromProjectileType;
            FromItemType = fromItemType;
            FromItemPrefix = fromItemPrefix;
            FromCustomReasonUtf8 = fromCustomReasonUtf8;
        }

        public PlayerDeathReasonFlags Flags { get; }

        public short FromPlayerIndex { get; }

        public short FromNpcIndex { get; }

        public short FromProjectileIndex { get; }

        public byte FromOther { get; }

        public short FromProjectileType { get; }

        public short FromItemType { get; }

        public byte FromItemPrefix { get; }

        public ReadOnlySpan<byte> FromCustomReasonUtf8 { get; }

        public bool HasPlayerIndex => (Flags & PlayerDeathReasonFlags.FromPlayerIndex) != 0;

        public bool HasNpcIndex => (Flags & PlayerDeathReasonFlags.FromNpcIndex) != 0;

        public bool HasProjectileIndex => (Flags & PlayerDeathReasonFlags.FromProjectileIndex) != 0;

        public bool HasOtherCause => (Flags & PlayerDeathReasonFlags.FromOther) != 0;

        public bool HasProjectileType => (Flags & PlayerDeathReasonFlags.FromProjectileType) != 0;

        public bool HasItemType => (Flags & PlayerDeathReasonFlags.FromItemType) != 0;

        public bool HasItemPrefix => (Flags & PlayerDeathReasonFlags.FromItemPrefix) != 0;

        public bool HasCustomReason => (Flags & PlayerDeathReasonFlags.FromCustomReason) != 0;

        public string GetCustomReason()
        {
            return Encoding.UTF8.GetString(FromCustomReasonUtf8);
        }

        public static PlayerDeathReasonView Parse(ref PacketReader reader)
        {
            ReadFields(
                ref reader,
                out PlayerDeathReasonFlags flags,
                out short fromPlayerIndex,
                out short fromNpcIndex,
                out short fromProjectileIndex,
                out byte fromOther,
                out short fromProjectileType,
                out short fromItemType,
                out byte fromItemPrefix,
                out ReadOnlySpan<byte> fromCustomReasonUtf8);

            return new PlayerDeathReasonView(
                flags,
                fromPlayerIndex,
                fromNpcIndex,
                fromProjectileIndex,
                fromOther,
                fromProjectileType,
                fromItemType,
                fromItemPrefix,
                fromCustomReasonUtf8);
        }

        public static void ReadFields(
            ref PacketReader reader,
            out PlayerDeathReasonFlags flags,
            out short fromPlayerIndex,
            out short fromNpcIndex,
            out short fromProjectileIndex,
            out byte fromOther,
            out short fromProjectileType,
            out short fromItemType,
            out byte fromItemPrefix,
            out ReadOnlySpan<byte> fromCustomReasonUtf8)
        {
            flags = (PlayerDeathReasonFlags)reader.ReadByte();
            fromPlayerIndex = MissingEntityIndex;
            fromNpcIndex = MissingEntityIndex;
            fromProjectileIndex = MissingEntityIndex;
            fromOther = NoOtherCause;
            fromProjectileType = 0;
            fromItemType = 0;
            fromItemPrefix = 0;
            fromCustomReasonUtf8 = default;

            if ((flags & PlayerDeathReasonFlags.FromPlayerIndex) != 0)
            {
                fromPlayerIndex = reader.ReadInt16();
            }

            if ((flags & PlayerDeathReasonFlags.FromNpcIndex) != 0)
            {
                fromNpcIndex = reader.ReadInt16();
            }

            if ((flags & PlayerDeathReasonFlags.FromProjectileIndex) != 0)
            {
                fromProjectileIndex = reader.ReadInt16();
            }

            if ((flags & PlayerDeathReasonFlags.FromOther) != 0)
            {
                fromOther = reader.ReadByte();
            }

            if ((flags & PlayerDeathReasonFlags.FromProjectileType) != 0)
            {
                fromProjectileType = reader.ReadInt16();
            }

            if ((flags & PlayerDeathReasonFlags.FromItemType) != 0)
            {
                fromItemType = reader.ReadInt16();
            }

            if ((flags & PlayerDeathReasonFlags.FromItemPrefix) != 0)
            {
                fromItemPrefix = reader.ReadByte();
            }

            if ((flags & PlayerDeathReasonFlags.FromCustomReason) != 0)
            {
                fromCustomReasonUtf8 = reader.ReadLengthPrefixedBytes();
            }
        }
    }
}
