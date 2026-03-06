using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class NpcHurtByDebuff : TerrariaPacket
        {
            private const short PayloadLength = 3;
    
            public byte NpcId { get; set; }
    
            public short Damage { get; set; }
    
            public NpcHurtByDebuff()
                : base((byte)PacketTypes.NpcHurtByDebuff)
            {
            }
    
            public NpcHurtByDebuff(BinaryReader br)
                : base(br)
            {
                NpcId = br.ReadByte();
                Damage = br.ReadInt16();
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
                    bw.Write(NpcId);
                    bw.Write(Damage);
                }
            }
        }
}

