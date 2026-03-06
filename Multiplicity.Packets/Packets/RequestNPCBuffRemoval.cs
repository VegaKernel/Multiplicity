using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class RequestNPCBuffRemoval : TerrariaPacket
        {
            private const short PayloadLength = 4;
    
            public short NpcId { get; set; }
    
            public ushort BuffId { get; set; }
    
            public RequestNPCBuffRemoval()
                : base((byte)PacketTypes.RequestNPCBuffRemoval)
            {
            }
    
            public RequestNPCBuffRemoval(BinaryReader br)
                : base(br)
            {
                NpcId = br.ReadInt16();
                BuffId = br.ReadUInt16();
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
                    bw.Write(BuffId);
                }
            }
        }
}

