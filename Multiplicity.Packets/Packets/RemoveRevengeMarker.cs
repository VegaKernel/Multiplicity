using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class RemoveRevengeMarker : TerrariaPacket
        {
            private const short PayloadLength = 4;
    
            public int UniqueId { get; set; }
    
            public RemoveRevengeMarker()
                : base((byte)PacketTypes.RemoveRevengeMarker)
            {
            }
    
            public RemoveRevengeMarker(BinaryReader br)
                : base(br)
            {
                UniqueId = br.ReadInt32();
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
                    bw.Write(UniqueId);
                }
            }
        }
}

