using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlayerDeath (0x2C) packet.
    /// </summary>
    public class PlayerDeath : TerrariaPacket
    {
        private const short FixedPayloadLength = 5;

        public byte PlayerID { get; set; }
        public byte HitDirection { get; set; }
        public short Damage { get; set; }
        public bool PVP { get; set; }
        public string DeathText { get; set; }

        public PlayerDeath()
            : base((byte)PacketTypes.PlayerDeath)
        {
            DeathText = string.Empty;
        }

        public PlayerDeath(BinaryReader br)
            : base(br)
        {
            PlayerID = br.ReadByte();
            HitDirection = br.ReadByte();
            Damage = br.ReadInt16();
            PVP = br.ReadBoolean();
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
                bw.Write(PlayerID);
                bw.Write(HitDirection);
                bw.Write(Damage);
                bw.Write(PVP);
                bw.Write(DeathText ?? string.Empty);
            }
        }

        public override string ToString()
        {
            return $"[PlayerDeath: PlayerID = {PlayerID} HitDirection = {HitDirection} Damage = {Damage} PVP = {PVP} DeathText = {DeathText}]";
        }
    }
}
