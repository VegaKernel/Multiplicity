using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlayerDeathV2 (76) packet.
    /// </summary>
    public class PlayerDeathV2 : TerrariaPacket
    {
        private const short BasePayloadLength = 5;
        private const short ShortPayloadLength = 2;
        private const short BytePayloadLength = 1;

        public const short MissingEntityIndex = -1;
        public const byte NoOtherCause = 254;

        public byte PlayerId { get; set; }
        public PlayerDeathReasonFlags PlayerDeathReason { get; set; }
        public short FromPlayerIndex { get; set; } = MissingEntityIndex;
        public short FromNpcIndex { get; set; } = MissingEntityIndex;
        public short FromProjectileIndex { get; set; } = MissingEntityIndex;
        public byte FromOther { get; set; } = NoOtherCause;
        public short FromProjectileType { get; set; }
        public short FromItemType { get; set; }
        public byte FromItemPrefix { get; set; }
        public string FromCustomReason { get; set; }
        public short Damage { get; set; }
        public byte HitDirection { get; set; }
        public byte Flags { get; set; }

        public PlayerDeathV2()
            : base((byte)PacketTypes.PlayerDeathV2)
        {
            FromCustomReason = string.Empty;
        }

        public PlayerDeathV2(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            PlayerDeathReason = (PlayerDeathReasonFlags)br.ReadByte();

            if (HasReason(PlayerDeathReasonFlags.FromPlayerIndex))
                FromPlayerIndex = br.ReadInt16();
            if (HasReason(PlayerDeathReasonFlags.FromNpcIndex))
                FromNpcIndex = br.ReadInt16();
            if (HasReason(PlayerDeathReasonFlags.FromProjectileIndex))
                FromProjectileIndex = br.ReadInt16();
            if (HasReason(PlayerDeathReasonFlags.FromOther))
                FromOther = br.ReadByte();
            if (HasReason(PlayerDeathReasonFlags.FromProjectileType))
                FromProjectileType = br.ReadInt16();
            if (HasReason(PlayerDeathReasonFlags.FromItemType))
                FromItemType = br.ReadInt16();
            if (HasReason(PlayerDeathReasonFlags.FromItemPrefix))
                FromItemPrefix = br.ReadByte();
            if (HasReason(PlayerDeathReasonFlags.FromCustomReason))
                FromCustomReason = br.ReadString();
            else
                FromCustomReason = string.Empty;

            Damage = br.ReadInt16();
            HitDirection = br.ReadByte();
            Flags = br.ReadByte();
        }

        public override short GetLength()
        {
            PlayerDeathReasonFlags reasonFlags = BuildReasonFlags();
            short packetLength = BasePayloadLength;

            if ((reasonFlags & PlayerDeathReasonFlags.FromPlayerIndex) != 0)
                packetLength += ShortPayloadLength;
            if ((reasonFlags & PlayerDeathReasonFlags.FromNpcIndex) != 0)
                packetLength += ShortPayloadLength;
            if ((reasonFlags & PlayerDeathReasonFlags.FromProjectileIndex) != 0)
                packetLength += ShortPayloadLength;
            if ((reasonFlags & PlayerDeathReasonFlags.FromOther) != 0)
                packetLength += BytePayloadLength;
            if ((reasonFlags & PlayerDeathReasonFlags.FromProjectileType) != 0)
                packetLength += ShortPayloadLength;
            if ((reasonFlags & PlayerDeathReasonFlags.FromItemType) != 0)
                packetLength += ShortPayloadLength;
            if ((reasonFlags & PlayerDeathReasonFlags.FromItemPrefix) != 0)
                packetLength += BytePayloadLength;
            if ((reasonFlags & PlayerDeathReasonFlags.FromCustomReason) != 0)
                packetLength += (short)PacketSerializationHelper.GetSerializedStringLength(FromCustomReason);

            return packetLength;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                PlayerDeathReasonFlags reasonFlags = BuildReasonFlags();
                bw.Write(PlayerId);
                bw.Write((byte)reasonFlags);

                if ((reasonFlags & PlayerDeathReasonFlags.FromPlayerIndex) != 0)
                    bw.Write(FromPlayerIndex);
                if ((reasonFlags & PlayerDeathReasonFlags.FromNpcIndex) != 0)
                    bw.Write(FromNpcIndex);
                if ((reasonFlags & PlayerDeathReasonFlags.FromProjectileIndex) != 0)
                    bw.Write(FromProjectileIndex);
                if ((reasonFlags & PlayerDeathReasonFlags.FromOther) != 0)
                    bw.Write(FromOther);
                if ((reasonFlags & PlayerDeathReasonFlags.FromProjectileType) != 0)
                    bw.Write(FromProjectileType);
                if ((reasonFlags & PlayerDeathReasonFlags.FromItemType) != 0)
                    bw.Write(FromItemType);
                if ((reasonFlags & PlayerDeathReasonFlags.FromItemPrefix) != 0)
                    bw.Write(FromItemPrefix);
                if ((reasonFlags & PlayerDeathReasonFlags.FromCustomReason) != 0)
                    bw.Write(FromCustomReason ?? string.Empty);

                bw.Write(Damage);
                bw.Write(HitDirection);
                bw.Write(Flags);
            }
        }

        public override string ToString()
        {
            return $"[PlayerDeathV2: PlayerId = {PlayerId} PlayerDeathReason = {PlayerDeathReason} FromPlayerIndex = {FromPlayerIndex} FromNpcIndex = {FromNpcIndex} FromProjectileIndex = {FromProjectileIndex} FromOther = {FromOther} FromProjectileType = {FromProjectileType} FromItemType = {FromItemType} FromItemPrefix = {FromItemPrefix} FromCustomReason = {FromCustomReason} Damage = {Damage} HitDirection = {HitDirection} Flags = {Flags}]";
        }

        private PlayerDeathReasonFlags BuildReasonFlags()
        {
            PlayerDeathReasonFlags reasonFlags = PlayerDeathReason;
            reasonFlags = SetReason(reasonFlags, PlayerDeathReasonFlags.FromPlayerIndex, FromPlayerIndex != MissingEntityIndex);
            reasonFlags = SetReason(reasonFlags, PlayerDeathReasonFlags.FromNpcIndex, FromNpcIndex != MissingEntityIndex);
            reasonFlags = SetReason(reasonFlags, PlayerDeathReasonFlags.FromProjectileIndex, FromProjectileIndex != MissingEntityIndex);
            reasonFlags = SetReason(reasonFlags, PlayerDeathReasonFlags.FromOther, FromOther != NoOtherCause);
            reasonFlags = SetReason(reasonFlags, PlayerDeathReasonFlags.FromProjectileType, FromProjectileType != 0);
            reasonFlags = SetReason(reasonFlags, PlayerDeathReasonFlags.FromItemType, FromItemType != 0);
            reasonFlags = SetReason(reasonFlags, PlayerDeathReasonFlags.FromItemPrefix, FromItemPrefix != 0);
            reasonFlags = SetReason(reasonFlags, PlayerDeathReasonFlags.FromCustomReason, !string.IsNullOrEmpty(FromCustomReason));
            return reasonFlags;
        }

        private bool HasReason(PlayerDeathReasonFlags flag)
        {
            return (PlayerDeathReason & flag) != 0;
        }

        private static PlayerDeathReasonFlags SetReason(PlayerDeathReasonFlags current, PlayerDeathReasonFlags flag, bool enabled)
        {
            return enabled ? current | flag : current & ~flag;
        }
    }
}
