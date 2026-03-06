using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class TELeashedEntityAnchorPlaceItem : TerrariaPacket
        {
            private const short PayloadLength = 6;
    
            public short TileX { get; set; }
    
            public short TileY { get; set; }
    
            public short ItemType { get; set; }
    
            public TELeashedEntityAnchorPlaceItem()
                : base((byte)PacketTypes.TELeashedEntityAnchorPlaceItem)
            {
            }
    
            public TELeashedEntityAnchorPlaceItem(BinaryReader br)
                : base(br)
            {
                TileX = br.ReadInt16();
                TileY = br.ReadInt16();
                ItemType = br.ReadInt16();
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
                    bw.Write(ItemType);
                }
            }
        }
}

