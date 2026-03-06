using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class SyncItemPosition : TerrariaPacket
        {
            private const short PayloadLength = 10;
    
            public short ItemId { get; set; }
    
            public float PositionX { get; set; }
    
            public float PositionY { get; set; }
    
            public SyncItemPosition()
                : base((byte)PacketTypes.SyncItemPosition)
            {
            }
    
            public SyncItemPosition(BinaryReader br)
                : base(br)
            {
                ItemId = br.ReadInt16();
                PositionX = br.ReadSingle();
                PositionY = br.ReadSingle();
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
                    bw.Write(ItemId);
                    bw.Write(PositionX);
                    bw.Write(PositionY);
                }
            }
        }
}

