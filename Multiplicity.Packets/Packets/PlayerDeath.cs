using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlayerDeath (0x2C) packet.
    /// </summary>
    public class PlayerDeath : TerrariaPacket
    {
        private const short FixedPayloadLength = 5;

        public byte PlayerId { get; set; }
        public byte HitDirection { get; set; }
        public short Damage { get; set; }
        public bool Pvp { get; set; }
        public string DeathText { get; set; }

        public PlayerDeath()
            : base((byte)PacketTypes.PlayerDeath)
        {
            DeathText = string.Empty;
        }

        public PlayerDeath(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            HitDirection = br.ReadByte();
            Damage = br.ReadInt16();
            Pvp = br.ReadBoolean();
            DeathText = br.ReadString();
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
                bw.Write(Pvp);
                bw.Write(DeathText ?? string.Empty);
            }
        }

        public override string ToString()
        {
            return $"[PlayerDeath: PlayerId = {PlayerId} HitDirection = {HitDirection} Damage = {Damage} Pvp = {Pvp} DeathText = {DeathText}]";
        }
    }
}

