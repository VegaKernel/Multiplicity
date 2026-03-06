using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class TeamChangeFromUI : TerrariaPacket
        {
            private const short PayloadLength = 2;
    
            public byte PlayerId { get; set; }
    
            public byte Team { get; set; }
    
            public TeamChangeFromUI()
                : base((byte)PacketTypes.TeamChangeFromUI)
            {
            }
    
            public TeamChangeFromUI(BinaryReader br)
                : base(br)
            {
                PlayerId = br.ReadByte();
                Team = br.ReadByte();
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
                    bw.Write(PlayerId);
                    bw.Write(Team);
                }
            }
        }
}

