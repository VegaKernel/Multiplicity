using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The NpcStrike (0x1C) packet for protocol 325.
    /// NPC identity is a slot/generation pair.
    /// </summary>
    public class NpcStrike : TerrariaPacket
    {
        public byte NpcSlot { get; set; }
        public byte Generation { get; set; }
        public short Damage { get; set; }
        public float Knockback { get; set; }
        public byte Direction { get; set; }
        public byte Crit { get; set; }

        public NpcStrike()
            : base((byte)PacketTypes.NpcStrike)
        {
        }

        public NpcStrike(BinaryReader br)
            : base(br)
        {
            NpcSlot = br.ReadByte();
            Generation = br.ReadByte();
            Damage = br.ReadInt16();
            Knockback = br.ReadSingle();
            Direction = br.ReadByte();
            Crit = br.ReadByte();
        }

        public override string ToString()
        {
            return $"[NpcStrike: NpcSlot = {NpcSlot} Generation = {Generation} Damage = {Damage} Knockback = {Knockback} Direction = {Direction} Crit = {Crit}]";
        }

        public override short GetLength() => 10;

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(NpcSlot);
                bw.Write(Generation);
                bw.Write(Damage);
                bw.Write(Knockback);
                bw.Write(Direction);
                bw.Write(Crit);
            }
        }
    }
}
