using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class RequestTileEntityInteraction : TerrariaPacket
        {
            private const short PayloadLength = 5;
    
            public int TileEntityId { get; set; }
    
            public byte PlayerId { get; set; }
    
            public RequestTileEntityInteraction()
                : base((byte)PacketTypes.RequestTileEntityInteraction)
            {
            }
    
            public RequestTileEntityInteraction(BinaryReader br)
                : base(br)
            {
                TileEntityId = br.ReadInt32();
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
                    bw.Write(TileEntityId);
                    bw.Write(PlayerId);
                }
            }
        }
}

