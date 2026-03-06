using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class RequestSection : TerrariaPacket
        {
            private const short PayloadLength = 4;
    
            public ushort SectionX { get; set; }
    
            public ushort SectionY { get; set; }
    
            public RequestSection()
                : base((byte)PacketTypes.RequestSection)
            {
            }
    
            public RequestSection(BinaryReader br)
                : base(br)
            {
                SectionX = br.ReadUInt16();
                SectionY = br.ReadUInt16();
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
                    bw.Write((short)SectionX);
                    bw.Write((short)SectionY);
                }
            }
        }
}

