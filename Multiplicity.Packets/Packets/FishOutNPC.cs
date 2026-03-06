using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class FishOutNPC : TerrariaPacket
        {
            private const short PayloadLength = 6;
    
            public ushort TileX { get; set; }
    
            public ushort TileY { get; set; }
    
            public short NpcNetId { get; set; }
    
            public FishOutNPC()
                : base((byte)PacketTypes.FishOutNPC)
            {
            }
    
            public FishOutNPC(BinaryReader br)
                : base(br)
            {
                TileX = br.ReadUInt16();
                TileY = br.ReadUInt16();
                NpcNetId = br.ReadInt16();
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
                    bw.Write(TileX);
                    bw.Write(TileY);
                    bw.Write(NpcNetId);
                }
            }
        }
}

