using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The WiredCannonShot (0x6C) packet.
    /// </summary>
    public class WiredCannonShot : TerrariaPacket
    {
        private const short PayloadLength = 15;

        public short Damage { get; set; }
        public float Knockback { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public short Angle { get; set; }
        public short Ammo { get; set; }
        public byte PlayerId { get; set; }

        public WiredCannonShot()
            : base((byte)PacketTypes.WiredCannonShot)
        {
        }

        public WiredCannonShot(BinaryReader br)
            : base(br)
        {
            Damage = br.ReadInt16();
            Knockback = br.ReadSingle();
            X = br.ReadInt16();
            Y = br.ReadInt16();
            Angle = br.ReadInt16();
            Ammo = br.ReadInt16();
            PlayerId = br.ReadByte();
        }

        public override short GetLength()
        {
            return PayloadLength;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(Damage);
                bw.Write(Knockback);
                bw.Write(X);
                bw.Write(Y);
                bw.Write(Angle);
                bw.Write(Ammo);
                bw.Write(PlayerId);
            }
        }

        public override string ToString()
        {
            return $"[WiredCannonShot: Damage = {Damage}, Knockback = {Knockback}, X = {X}, Y = {Y}, Angle = {Angle}, Ammo = {Ammo}, PlayerId = {PlayerId}]";
        }
    }
}

