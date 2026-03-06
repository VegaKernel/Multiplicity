using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlayerDamage (0x1A) packet.
    /// </summary>
    public class PlayerDamage : TerrariaPacket
    {
        private const short FixedPayloadLength = 5;

        public byte PlayerId { get; set; }
        public byte HitDirection { get; set; }
        public short Damage { get; set; }
        public string DeathText { get; set; }
        public byte Flags { get; set; }

        public PlayerDamage()
            : base((byte)PacketTypes.PlayerDamage)
        {
            DeathText = string.Empty;
        }

        public PlayerDamage(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            HitDirection = br.ReadByte();
            Damage = br.ReadInt16();
            DeathText = br.ReadString();
            Flags = br.ReadByte();
        }

        public override short GetLength()
        {
            return (short)(FixedPayloadLength + PacketSerializationHelper.GetSerializedStringLength(DeathText));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(PlayerId);
                bw.Write(HitDirection);
                bw.Write(Damage);
                bw.Write(DeathText ?? string.Empty);
                bw.Write(Flags);
            }
        }

        public override string ToString()
        {
            return $"[PlayerDamage: PlayerId = {PlayerId} HitDirection = {HitDirection} Damage = {Damage} DeathText = {DeathText} Flags = {Flags}]";
        }
    }
}

